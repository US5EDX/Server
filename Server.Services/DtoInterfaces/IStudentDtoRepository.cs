using Server.Services.Dtos;

namespace Server.Services.DtoInterfaces
{
    public interface IStudentDtoRepository
    {
        Task<IEnumerable<StudentWithAllRecordsInfo>> GetWithAllRecordsByGroupId(uint groupId);
    }
}