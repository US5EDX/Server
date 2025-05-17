using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories;

public class UserRepository(ElCoursesDbContext context) : IUserRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<User?> GetByEmail(string email) => await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<User?> GetByToken(string refreshToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

    public async Task<User?> GetById(byte[] id) => await _context.Users.SingleOrDefaultAsync(u => u.UserId.SequenceEqual(id));

    public async Task<Student?> GetStudent(byte[] id) =>
        await _context.Students.Include(st => st.FacultyNavigation)
            .Include(st => st.GroupNavigation).SingleOrDefaultAsync(st => st.StudentId.SequenceEqual(id));

    public async Task<Worker?> GetWorker(byte[] id) =>
        await _context.Workers.Include(wr => wr.FacultyNavigation).SingleOrDefaultAsync(wr => wr.WorkerId.SequenceEqual(id));

    public async Task RefreshToken(User user, string refreshToken, DateTime expiry)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTokenAsync(User user)
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