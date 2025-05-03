using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Models;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;
using Server.Services.Services;
using System.Drawing;
using System.Linq.Expressions;

namespace Server.Data.Repositories
{
    public class DisciplineDtoRepository : IDisciplineDtoRepository
    {
        private readonly ElCoursesDbContext _context;

        public DisciplineDtoRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DisciplineShortInfoDto>> GetByCodeSearchWithClosed
            (string code, short eduYear, byte eduLevel, byte semester)
        {
            return await GetByCodeSearch(d => d.Holding == eduYear && d.EduLevel == eduLevel &&
                (d.Semester == 0 || d.Semester == semester) && d.DisciplineCode.StartsWith(code));
        }

        public async Task<IEnumerable<DisciplineShortInfoDto>> GetByCodeSearchWithoutClosed
            (string code, short eduYear, byte eduLevel, byte courseMask, byte semester)
        {
            return await GetByCodeSearch(d => d.Holding == eduYear && d.EduLevel == eduLevel && (d.Course & courseMask) > 0 &&
                (d.Semester == 0 || d.Semester == semester) && d.IsOpen == true && d.DisciplineCode.StartsWith(code));
        }

        public async Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines
            (int page, int size, uint facultyId, short eduYear)
        {
            return await GetWithSubscribersAsync(_context.Disciplines
                                 .Where(d => d.FacultyId == facultyId && d.Holding == eduYear),
                                 page, size);
        }

        public async Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines
            (int page, int size, uint facultyId, short eduYear, byte catalogType)
        {
            return await GetWithSubscribersAsync(_context.Disciplines
                                 .Where(d => d.FacultyId == facultyId && d.Holding == eduYear && d.CatalogType == catalogType),
                                 page, size);
        }

        public async Task<IEnumerable<DisciplineInfoForStudent>> GetDisciplinesForStudent(int pageNumber, int pageSize,
            byte eduLevel, short holding, byte catalogFilter, byte courseMask, byte semesterFilter, uint? facultyFilter)
        {
            var query = _context.Disciplines
                .Where(d => d.EduLevel == eduLevel && d.CatalogType == catalogFilter && d.IsOpen &&
                (d.Course & courseMask) > 0 && d.Holding == holding);

            if (semesterFilter != 0)
                query = query.Where(d => d.Semester == 0 || d.Semester == semesterFilter);

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
                    Course = ParserService.GetCourseString(d.Course),
                    Semester = d.Semester,
                    IsYearLong = d.IsYearLong,
                    Faculty = FacultyMapper.MapToFacultyDto(d.Faculty),
                }).ToArrayAsync();
        }

        public async Task<IEnumerable<DisciplinePrintInfo>> GetDisciplinesOnSemester(uint facultyId, byte catalogType, short eduYear, byte semester)
        {
            return await _context.Disciplines
                .Where(d => d.FacultyId == facultyId && d.CatalogType == catalogType && d.Holding == eduYear
                && (d.Semester == 0 || d.Semester == semester))
                .Select(d => new DisciplinePrintInfo()
                {
                    DisciplineCode = d.DisciplineCode,
                    DisciplineName = d.DisciplineName,
                    StudentsCount = d.Records.Count(r => r.Holding == d.Holding && r.Semester == semester),
                    SpecialtyName = d.Specialty.SpecialtyName,
                    EduLevel = d.EduLevel,
                    Course = ParserService.GetCourseString(d.Course),
                    Semester = d.Semester,
                    MinCount = d.MinCount,
                    MaxCount = d.MaxCount,
                    IsOpen = d.IsOpen,
                }).ToListAsync();
        }

        private async Task<IEnumerable<DisciplineShortInfoDto>> GetByCodeSearch(Expression<Func<Discipline, bool>> wherePredicate)
        {
            return await _context.Disciplines
                .Where(wherePredicate)
                .Select(r => new DisciplineShortInfoDto()
                {
                    DisciplineId = r.DisciplineId,
                    DisciplineCodeName = $"{r.DisciplineCode} {r.DisciplineName}",
                    IsYearLong = r.IsYearLong,
                })
                .ToListAsync();
        }

        private async Task<IEnumerable<DisciplineWithSubCountDto>> GetWithSubscribersAsync
            (IQueryable<Discipline> disciplinesQueryable, int page, int size)
        {
            return await disciplinesQueryable
                                 .OrderBy(d => d.DisciplineCode)
                                 .Skip((page - 1) * size)
                                 .Take(size)
                                 .Select(d => new DisciplineWithSubCountDto
                                 {
                                     DisciplineId = d.DisciplineId,
                                     DisciplineCode = d.DisciplineCode,
                                     CatalogType = d.CatalogType,
                                     Faculty = FacultyMapper.MapToFacultyDto(d.Faculty),
                                     Specialty = SpecialtyMapper.MapToSpecialtyDto(d.Specialty),
                                     DisciplineName = d.DisciplineName,
                                     EduLevel = d.EduLevel,
                                     Course = ParserService.GetCourseString(d.Course),
                                     Semester = d.Semester,
                                     Prerequisites = d.Prerequisites,
                                     Interest = d.Interest,
                                     MaxCount = d.MaxCount,
                                     MinCount = d.MinCount,
                                     Url = d.Url,
                                     NonparsemesterCount = d.Records.Count(r => r.Holding == d.Holding && r.Semester == 1),
                                     ParsemesterCount = d.Records.Count(r => r.Holding == d.Holding && r.Semester == 2),
                                     Holding = d.Holding,
                                     IsYearLong = d.IsYearLong,
                                     IsOpen = d.IsOpen,
                                 })
                                 .ToListAsync();
        }
    }
}
