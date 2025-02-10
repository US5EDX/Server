using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ElCoursesDbContext _context;

        public UserRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetUserByToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> GetUserById(byte[] id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<Student?> GetStudent(byte[] id)
        {
            return await _context.Students.Include(st => st.FacultyNavigation)
                .Include(st => st.GroupNavigation).FirstOrDefaultAsync(st => st.StudentId == id);
        }

        public async Task<Worker?> GetWorker(byte[] id)
        {
            return await _context.Workers.Include(wr => wr.FacultyNavigation).FirstOrDefaultAsync(wr => wr.WorkerId == id);
        }

        public async Task RefreshToken(User user, string refreshToken, DateTime expiry)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToken(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePassword(User user, string password, string salt)
        {
            user.Password = password;
            user.Salt = salt;
            await _context.SaveChangesAsync();
        }
    }
}
