using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IAcademicianRepository
    {
        Task<int> GetCount(uint facultyId);
        Task<int> GetCount(uint facultyId, byte roleFilter);
        Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyId);
        Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyId, byte roleFilter);
    }
}