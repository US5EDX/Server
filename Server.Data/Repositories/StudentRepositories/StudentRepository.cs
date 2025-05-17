using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories.StudentRepositories;

public class StudentRepository(ElCoursesDbContext context) : IStudentRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<Student>> GetWithLastReocorsByGroupId(uint groupId, short holding) =>
        await _context.Students
            .Include(s => s.User)
            .Include(s => s.Records.Where(r => r.Holding == holding)).ThenInclude(r => r.Discipline)
            .Where(s => s.Group == groupId)
            .Select(s => new Student()
            {
                StudentId = s.StudentId,
                User = new User() { Email = s.User.Email },
                FullName = s.FullName,
                Headman = s.Headman,
                Records = s.Records.Select(r => new Record()
                {
                    Semester = r.Semester,
                    Discipline = new Discipline()
                    {
                        DisciplineCode = r.Discipline.DisciplineCode,
                        DisciplineName = r.Discipline.DisciplineName
                    },
                    Approved = r.Approved
                }).ToList()
            }).ToListAsync();

    public async Task<User> Add(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.Users.AddAsync(user);
        await _context.Students.AddAsync(user.Student);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return user;
    }

    public async Task<IReadOnlyList<User>> AddRange(List<User> users, IEnumerable<Student> studentPart)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.Users.AddRangeAsync(users);
        await _context.Students.AddRangeAsync(studentPart);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return users;
    }

    public async Task<Student?> Update(Student student)
    {
        var existingStudent = await _context.Students.Include(s => s.User)
            .SingleOrDefaultAsync(s => s.StudentId.SequenceEqual(student.StudentId));

        if (existingStudent is null) return null;

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

    public async Task<DeleteResultEnum> Delete(byte[] userId, Roles requestUserRole, short deleteBorderYear)
    {
        var existingUser = await _context.Users.Include(u => u.Student)
            .SingleOrDefaultAsync(u => u.UserId.SequenceEqual(userId));

        if (existingUser is null || existingUser.Student is null || existingUser.Role <= requestUserRole)
            return DeleteResultEnum.ValueNotFound;

        bool hasDependencies =
            await _context.Records.AnyAsync(r => r.StudentId.SequenceEqual(existingUser.UserId) && r.Holding > deleteBorderYear);

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        _context.Users.Remove(existingUser);
        await _context.SaveChangesAsync();

        return DeleteResultEnum.Success;
    }
}
