using Server.Models.Models;

namespace Server.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByToken(string refreshToken);
        Task<Student?> GetStudent(byte[] id);
        Task<Worker?> GetWorker(byte[] id);
        Task<User?> GetUserById(byte[] id);
        Task RefreshToken(User user, string RefreshToken, DateTime expiry);
        Task DeleteToken(User user);
        Task UpdatePassword(User user, string password, string salt);
    }
}