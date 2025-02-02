using Server.Models.Models;

namespace Server.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<Student?> GetStudent(byte[] id);
        Task<Worker?> GetWorker(byte[] id);
        Task<User?> GetUserByToken(string refreshToken);
        Task RefreshToken(User user, string RefreshToken, DateTime expiry);
        Task DeleteToken(User user);
    }
}