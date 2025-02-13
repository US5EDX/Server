using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IWorkerRepository
    {
        Task<int> GetCount();
        Task<int> GetCount(uint facultyFilter);
        Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize);
        Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyFilter);
        Task<User> Add(User user);
        Task<User?> Update(User user);
        Task<bool?> Delete(byte[] userId);
    }
}