using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetByToken(string refreshToken);
    Task<Student?> GetStudent(byte[] id);
    Task<Worker?> GetWorker(byte[] id);
    Task<User?> GetById(byte[] id);
    Task RefreshToken(User user, string RefreshToken, DateTime expiry);
    Task DeleteTokenAsync(User user);
    Task UpdatePassword(User user, string password, string salt);
}