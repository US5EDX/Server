using Server.Services.Dtos;

namespace Server.Services.DtoInterfaces
{
    public interface IDisciplineDtoRepository
    {
        Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines(int page, int size, uint facultyId, short eduYear);
        Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines(int page, int size, uint facultyId, short eduYear, byte catalogType);
        Task<IEnumerable<DisciplinePrintInfo>> GetDisciplinesOnSemester(uint facultyId, byte catalogType, short eduYear, byte semester);
    }
}