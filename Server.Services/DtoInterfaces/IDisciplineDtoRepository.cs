using Server.Services.Dtos;

namespace Server.Services.DtoInterfaces
{
    public interface IDisciplineDtoRepository
    {
        Task<IEnumerable<DisciplineShortInfoDto>> GetByCodeSearchWithClosed(string code, short eduYear, byte eduLevel, byte semester);
        Task<IEnumerable<DisciplineShortInfoDto>> GetByCodeSearchWithoutClosed(string code, short eduYear, byte eduLevel, byte semester);
        Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines(int page, int size, uint facultyId, short eduYear);
        Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines(int page, int size, uint facultyId, short eduYear, byte catalogType);
        Task<IEnumerable<DisciplinePrintInfo>> GetDisciplinesOnSemester(uint facultyId, byte catalogType, short eduYear, byte semester);
    }
}