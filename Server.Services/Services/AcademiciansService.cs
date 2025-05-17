using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Dtos.UserDtos;
using Server.Services.Mappings;

namespace Server.Services.Services;

public class AcademiciansService(IAcademicianRepository academicianRepository)
{
    public async Task<int> GetCount(uint facultyId, Roles? roleFilter) =>
        await academicianRepository.GetCount(facultyId, roleFilter);

    public async Task<IEnumerable<UserFullInfoDto>> GetAcademicians(int pageNumber, int pageSize, uint facultyId, Roles? roleFilter)
    {
        var workers = await academicianRepository.GetAcademicians(pageNumber, pageSize, facultyId, roleFilter);

        return workers.Select(UserMapper.MapToUserFullInfoDto);
    }
}
