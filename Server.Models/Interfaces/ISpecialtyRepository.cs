using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface ISpecialtyRepository
{
    Task<IReadOnlyList<Specialty>> GetByFacultyId(uint facultyId);
    Task<Specialty> Add(Specialty specialty);
    Task<Specialty?> Update(Specialty specialty);
    Task<DeleteResultEnum> DeleteAsync(uint specialtyId);
}