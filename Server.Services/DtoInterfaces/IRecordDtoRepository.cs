using Server.Services.Dtos;

namespace Server.Services.DtoInterfaces
{
    public interface IRecordDtoRepository
    {
        Task<IEnumerable<RecordWithDisciplineInfoDto>> GetByStudentIdAndYear(byte[] studentId, short year);
        Task<RecordWithDisciplineInfoDto?> GetWithDisciplineById(uint recordId);
        Task<IEnumerable<RecordShortDisciplineInfoDto>> GetWithDisciplineShort(byte[] studentId, short year);
        Task<IEnumerable<StudentYearsRecordsDto>> GetStudentRecordsByYears(byte[] studentId, HashSet<short> years);
    }
}