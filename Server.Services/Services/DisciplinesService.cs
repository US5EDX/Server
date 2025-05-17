using Microsoft.Extensions.Options;
using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Converters;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.DisciplineDtos;
using Server.Services.Mappings;
using Server.Services.Parsers;

namespace Server.Services.Services;

public class DisciplinesService(IDisciplineRepository disciplineRepository, IDisciplineDtoRepository disciplineDtoRepository,
    IOptions<DisciplineStatusThresholds> disciplineStatusThresholds, IOptions<DisciplineStatusColors> disciplineStatusColors)
{
    private readonly DisciplineStatusThresholds _disciplineStatusThresholds = disciplineStatusThresholds.Value;
    private readonly DisciplineStatusColors _disciplineStatusColors = disciplineStatusColors.Value;

    public async Task<int> GetCount(uint facultyId, short holdingFilter,
        CatalogTypes? catalogFilter, Semesters? semesterFilter, string? lecturerFilter)
    {
        var lecturerByteFilter = UlidIdParser.ParseIdWithNull(lecturerFilter);

        return await disciplineRepository.GetCount(facultyId, holdingFilter, catalogFilter, semesterFilter, lecturerByteFilter);
    }

    public async Task<IReadOnlyList<DisciplineWithSubCountDto>> GetDisciplines(int pageNumber, int pageSize,
        uint facultyId, short holdingFilter, CatalogTypes? catalogFilter, Semesters? semesterFilter, string? lecturerFilter)
    {
        var lecturerByteFilter = UlidIdParser.ParseIdWithNull(lecturerFilter);

        return await disciplineDtoRepository.Get(pageNumber, pageSize, facultyId,
            holdingFilter, catalogFilter, semesterFilter, lecturerByteFilter);
    }

    public async Task<int> GetCountForStudent(EduLevels eduLevel, short holding,
        CatalogTypes catalogFilter, byte courseFilter, Semesters semesterFilter, uint? facultyFilter)
    {
        byte courseMask = CourseToCourseMaskConverter.ConvertToCourseMask(courseFilter);

        return await disciplineRepository.GetCountForStudent(eduLevel, holding,
            catalogFilter, courseMask, semesterFilter, facultyFilter);
    }

    public async Task<IReadOnlyList<DisciplineInfoForStudent>> GetDisciplinesForStudent(int pageNumber, int pageSize,
        EduLevels eduLevel, short holding, CatalogTypes catalogFilter, byte courseFilter,
        Semesters semesterFilter, uint? facultyFilter)
    {
        byte courseMask = CourseToCourseMaskConverter.ConvertToCourseMask(courseFilter);

        return await disciplineDtoRepository.GetForStudentAsync(
            pageNumber, pageSize, eduLevel, holding, catalogFilter, courseMask, semesterFilter, facultyFilter);
    }

    public async Task<DisciplineFullInfoDto?> GetByIdOrThrow(uint disciplineId)
    {
        var discipline = await disciplineRepository.GetById(disciplineId) ?? throw new NotFoundException("Дисципліну не знайдено");

        return DisciplineMapper.MapToDisciplineFullInfoDto(discipline);
    }

    public async Task<IReadOnlyList<DisciplineShortInfoDto>> GetByCodeSearchYearEduLevelSemester(
        string code, short year, EduLevels eduLevel, Semesters semester) =>
        await disciplineDtoRepository.GetSearchResultsForAdmin(code, year, eduLevel, semester);

    public async Task<IReadOnlyList<DisciplineShortInfoDto>> GetOptionsByCodeSearch(
        string code, short year, EduLevels eduLevel, byte course, Semesters semester)
    {
        byte courseMask = CourseToCourseMaskConverter.ConvertToCourseMask(course);

        return await disciplineDtoRepository.GetSearchResultsForStudent(code, year, eduLevel, courseMask, semester);
    }

    public DisciplineStatusThresholds GetThresholds() => _disciplineStatusThresholds;

    public async Task<object> GetDisciplinesPrintInfo(uint facultyId, CatalogTypes catalogType, short eduYear, Semesters semester)
    {
        var disciplines = await disciplineDtoRepository.GetOnSemester(facultyId, catalogType, eduYear, semester);

        foreach (var discipline in disciplines)
            discipline.ColorStatus = _disciplineStatusColors.GetColor(discipline.StudentsCount, _disciplineStatusThresholds);

        return new { Thresholds = _disciplineStatusThresholds, Disciplines = disciplines };
    }

    public async Task<DisciplineFullInfoDto> AddDisciplineOrThrow(DisciplineRegistryDto discipline, string? userId)
    {
        if (userId is null) throw new BadRequestException("Неможливо виконати дію");

        discipline.DisciplineId = 0;
        discipline.IsOpen = true;

        var newDiscipline = DisciplineMapper.MapToDiscipline(discipline);
        newDiscipline.CreatorId = UlidIdParser.ParseId(userId);

        var addedDiscipline = await disciplineRepository.Add(newDiscipline);

        return DisciplineMapper.MapToDisciplineFullInfoDto(addedDiscipline);
    }

    public async Task<DisciplineFullInfoDto> UpdateOrThrow(DisciplineRegistryDto discipline)
    {
        if (discipline.DisciplineId == 0) throw new BadRequestException("Невалідні дані");

        var updatedDiscipline = await disciplineRepository.Update(DisciplineMapper.MapToDiscipline(discipline)) ??
            throw new NotFoundException("Дисципліну не знайдено");

        return DisciplineMapper.MapToDisciplineFullInfoDto(updatedDiscipline);
    }

    public async Task DeleteOrThrow(uint disciplineId)
    {
        var result = await disciplineRepository.Delete(disciplineId, _disciplineStatusThresholds.NotEnough);

        result.ThrowIfFailed("Дисципліну не знайдено", "Дисципліна як мінумум умовно набрана");
    }

    public async Task UpdateStatusOrThrow(uint disciplineId)
    {
        var isSuccess = await disciplineRepository.UpdateStatus(disciplineId);

        if (!isSuccess) throw new NotFoundException("Дисципліну не знайдено");
    }
}
