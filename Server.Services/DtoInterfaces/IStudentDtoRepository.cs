using Server.Services.Dtos.StudentDtos;

namespace Server.Services.DtoInterfaces;

public interface IStudentDtoRepository
{
    Task<IReadOnlyList<StudentWithAllRecordsInfo>> GetWithAllRecordsByGroupId(uint groupId);
}