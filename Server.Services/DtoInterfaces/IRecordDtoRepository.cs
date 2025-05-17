using Server.Models.Enums;
using Server.Services.Dtos.RecordDtos;
using Server.Services.Dtos.StudentDtos;

namespace Server.Services.DtoInterfaces;

public interface IRecordDtoRepository
{
    Task<IReadOnlyList<RecordWithDisciplineInfoDto>> GetByStudentIdAndYear(byte[] studentId, short year);
    Task<RecordWithDisciplineInfoDto?> GetWithDisciplineById(uint recordId);
    Task<IReadOnlyList<RecordShortDisciplineInfoDto>> GetWithDisciplineShort(byte[] studentId, short year);
    Task<IReadOnlyList<StudentYearsRecordsDto>> GetStudentRecordsByYears(byte[] studentId, IReadOnlyList<short> years);
    Task<IReadOnlyList<StudentYearsRecordsDto>> GetMadeChoices(byte[] studentId);
    Task<IReadOnlyList<RecordWithStudentInfoDto>> GetRecordsWithStudentInfo(uint disciplineId, Semesters semester);
}