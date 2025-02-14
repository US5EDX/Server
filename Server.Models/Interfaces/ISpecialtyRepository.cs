using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<Specialty> Add(Specialty specialty);
        Task<bool?> Delete(uint specialtyId);
        Task<IEnumerable<Specialty>> GetByFacultyId(uint facultyId);
        Task<Specialty?> Update(Specialty specialty);
    }
}