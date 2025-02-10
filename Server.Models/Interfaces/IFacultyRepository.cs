using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculty>> GetAll();
        Task<Faculty> Add(Faculty faculty);
        Task<Faculty?> Update(Faculty faculty);
        Task<bool?> Delete(uint facultyId);
    }
}