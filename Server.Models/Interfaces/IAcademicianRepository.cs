using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IAcademicianRepository
{
    Task<int> GetCount(uint facultyId, Roles? roleFilter);
    Task<IReadOnlyList<User>> GetAcademicians(int pageNumber, int pageSize, uint facultyId, Roles? roleFilter);
}