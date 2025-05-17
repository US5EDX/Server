using Server.Models.Enums;
using Server.Services.Dtos.DisciplineDtos;

namespace Server.Services.DtoInterfaces;

public interface IDisciplineDtoRepository
{
    Task<IReadOnlyList<DisciplineShortInfoDto>> GetSearchResultsForAdmin(string code, short eduYear,
        EduLevels eduLevel, Semesters semester);
    Task<IReadOnlyList<DisciplineShortInfoDto>> GetSearchResultsForStudent(string code, short eduYear,
        EduLevels eduLevel, byte couseMask, Semesters semester);
    Task<IReadOnlyList<DisciplineWithSubCountDto>> Get(int page, int size, uint facultyId, short eduYear,
        CatalogTypes? catalogType, Semesters? semesterFilter, byte[]? creatorFilter);
    Task<IReadOnlyList<DisciplineInfoForStudent>> GetForStudentAsync(int pageNumber, int pageSize, EduLevels eduLevel,
        short holding, CatalogTypes catalogFilter, byte courseMask, Semesters semesterFilter, uint? facultyFilter);
    Task<IReadOnlyList<DisciplinePrintInfo>> GetOnSemester(uint facultyId, CatalogTypes catalogType,
        short eduYear, Semesters semester);
}