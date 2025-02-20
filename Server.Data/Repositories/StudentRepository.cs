using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ElCoursesDbContext _context;

        public StudentRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetWithLastReocorsByGroupId(uint groupId, short holding)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.Records.Where(r => r.Holding == holding))
                .ThenInclude(r => r.Discipline)
                .Where(s => s.Group == groupId)
                .ToListAsync();
        }

        public async Task<User> Add(User user)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.Users.AddAsync(user);

            if (user.Student != null)
            {
                await _context.Students.AddAsync(user.Student);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return user;
        }

        public async Task<IEnumerable<User>> AddRange(List<User> users)
        {
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            return users;
        }

        public async Task<Student?> Update(Student student)
        {
            var existingStudent = await _context.Students.Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId.SequenceEqual(student.StudentId));

            if (existingStudent is null)
                return null;

            if (existingStudent.User.Email != student.User.Email)
            {
                student.User.RefreshToken = null;
                student.User.RefreshTokenExpiry = null;
            }

            existingStudent.User.Email = student.User.Email;
            existingStudent.FullName = student.FullName;
            existingStudent.Headman = student.Headman;

            await _context.SaveChangesAsync();

            return existingStudent;
        }

        public async Task<bool?> Delete(byte[] userId, int requestUserRole)
        {
            var existingUser = await _context.Users
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.UserId.SequenceEqual(userId));

            if (existingUser is null || existingUser.Student is null || existingUser.Role <= requestUserRole)
                return false;

            bool hasDependencies =
                await _context.Records.AnyAsync(r => r.StudentId.SequenceEqual(existingUser.Student.StudentId) &&
                r.Holding > (DateTime.Today.Year - 2));

            if (hasDependencies)
                return null;

            if (existingUser.Student is not null)
            {
                _context.Students.Remove(existingUser.Student);
            }

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
