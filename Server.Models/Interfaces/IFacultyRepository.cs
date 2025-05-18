using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IFacultyRepository
{
    Task<IReadOnlyList<Faculty>> GetAll();
    Task<Faculty> Add(Faculty faculty);
    Task<bool> Update(Faculty faculty);
    Task<DeleteResultEnum> Delete(uint facultyId);
}