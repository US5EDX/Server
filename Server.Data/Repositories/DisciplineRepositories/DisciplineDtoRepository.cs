using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.DisciplineDtos;
using Server.Services.Mappings;
using System.Linq.Expressions;

namespace Server.Data.Repositories.DisciplineRepositories;

public class DisciplineDtoRepository(ElCoursesDbContext context) : IDisciplineDtoRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<DisciplineShortInfoDto>> GetSearchResultsForAdmin
        (string code, short eduYear, EduLevels eduLevel, Semesters semester) =>
        await GetByCodeSearchAsync(d => d.Holding == eduYear && d.EduLevel == eduLevel &&
                (d.Semester == Semesters.Both || d.Semester == semester) && d.DisciplineCode.StartsWith(code));

    public async Task<IReadOnlyList<DisciplineShortInfoDto>> GetSearchResultsForStudent
        (string code, short eduYear, EduLevels eduLevel, byte courseMask, Semesters semester) =>
        await GetByCodeSearchAsync(d => d.Holding == eduYear && d.EduLevel == eduLevel && (d.Course & courseMask) > 0 &&
                (d.Semester == Semesters.Both || d.Semester == semester) && d.IsOpen == true && d.DisciplineCode.StartsWith(code));

    public async Task<IReadOnlyList<DisciplineWithSubCountDto>> Get
        (int page, int size, uint facultyId, short eduYear, CatalogTypes? catalogType, Semesters? semesterFilter, byte[]? creatorFilter)
    {
        var query = _context.Disciplines
                             .Where(d => d.FacultyId == facultyId && d.Holding == eduYear);

        if (catalogType is not null)
            query = query.Where(d => d.CatalogType == catalogType);

        if (semesterFilter is not null)
            query = query.Where(d => d.Semester == Semesters.Both || d.Semester == semesterFilter);

        if (creatorFilter is not null)
            query = query.Where(d => d.CreatorId.SequenceEqual(creatorFilter));

        return await query
            .OrderBy(d => d.DisciplineCode)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(d => new DisciplineWithSubCountDto
            {
                DisciplineId = d.DisciplineId,
                DisciplineCode = d.DisciplineCode,
                CatalogType = d.CatalogType,
                Faculty = FacultyMapper.MapToFacultyDto(d.Faculty),
                Specialty = SpecialtyMapper.MapToNullableSpecialtyDto(d.Specialty),
                DisciplineName = d.DisciplineName,
                EduLevel = d.EduLevel,
                Course = CourseMaskConverter.GetCourseMaskString(d.Course),
                Semester = d.Semester,
                Prerequisites = d.Prerequisites,
                Interest = d.Interest,
                MaxCount = d.MaxCount,
                MinCount = d.MinCount,
                Url = d.Url,
                NonparsemesterCount = d.Records.Count(r => r.Holding == d.Holding && r.Semester == Semesters.Fall),
                ParsemesterCount = d.Records.Count(r => r.Holding == d.Holding && r.Semester == Semesters.Spring),
                Holding = d.Holding,
                IsYearLong = d.IsYearLong,
                IsOpen = d.IsOpen,
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyList<DisciplineInfoForStudent>> GetForStudentAsync(int pageNumber, int pageSize,
        EduLevels eduLevel, short holding, CatalogTypes catalogFilter, byte courseMask, Semesters semesterFilter, uint? facultyFilter)
    {
        var query = _context.Disciplines
            .Where(d => d.EduLevel == eduLevel && d.CatalogType == catalogFilter && d.IsOpen &&
            (d.Course & courseMask) > 0 && d.Holding == holding);

        if (semesterFilter != Semesters.Both)
            query = query.Where(d => d.Semester == Semesters.Both || d.Semester == semesterFilter);

        if (facultyFilter is not null)
            query = query.Where(d => d.FacultyId == facultyFilter);

        return await query
            .OrderBy(d => d.DisciplineCode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DisciplineInfoForStudent()
            {
                DisciplineId = d.DisciplineId,
                DisciplineCode = d.DisciplineCode,
                DisciplineName = d.DisciplineName,
                Course = CourseMaskConverter.GetCourseMaskString(d.Course),
                Semester = d.Semester,
                IsYearLong = d.IsYearLong,
                Faculty = FacultyMapper.MapToFacultyDto(d.Faculty),
            }).ToListAsync();
    }

    public async Task<IReadOnlyList<DisciplinePrintInfo>> GetOnSemester(uint facultyId, CatalogTypes catalogType,
        short eduYear, Semesters semester) =>
        await _context.Disciplines
            .Where(d => d.FacultyId == facultyId && d.CatalogType == catalogType && d.Holding == eduYear
            && (d.Semester == 0 || d.Semester == semester))
            .Select(d => new DisciplinePrintInfo()
            {
                DisciplineCode = d.DisciplineCode,
                DisciplineName = d.DisciplineName,
                StudentsCount = d.Records.Count(r => r.Holding == d.Holding && r.Semester == semester),
                SpecialtyName = d.Specialty.SpecialtyName,
                EduLevel = d.EduLevel,
                Course = CourseMaskConverter.GetCourseMaskString(d.Course),
                Semester = d.Semester,
                MinCount = d.MinCount,
                MaxCount = d.MaxCount,
                IsOpen = d.IsOpen,
            }).ToListAsync();

    private async Task<IReadOnlyList<DisciplineShortInfoDto>> GetByCodeSearchAsync(Expression<Func<Discipline, bool>> wherePredicate) =>
        await _context.Disciplines
            .Where(wherePredicate)
            .Select(d => new DisciplineShortInfoDto()
            {
                DisciplineId = d.DisciplineId,
                DisciplineCodeName = $"{d.DisciplineCode} {d.DisciplineName}",
                IsYearLong = d.IsYearLong,
            })
            .ToListAsync();
}
