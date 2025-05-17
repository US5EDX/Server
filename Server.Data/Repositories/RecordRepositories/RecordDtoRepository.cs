using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.RecordDtos;
using Server.Services.Dtos.StudentDtos;
using System.Linq.Expressions;

namespace Server.Data.Repositories.RecordRepositories;

public class RecordDtoRepository(ElCoursesDbContext context) : IRecordDtoRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<RecordWithDisciplineInfoDto>> GetByStudentIdAndYear(byte[] studentId, short year) =>
        await GetQueryableRecordsWithDiscipline(r => r.Holding == year && r.StudentId.SequenceEqual(studentId))
            .ToListAsync();

    public async Task<RecordWithDisciplineInfoDto?> GetWithDisciplineById(uint recordId) =>
        await GetQueryableRecordsWithDiscipline(r => r.RecordId == recordId)
            .SingleOrDefaultAsync();

    public async Task<IReadOnlyList<StudentYearsRecordsDto>> GetStudentRecordsByYears(byte[] studentId, IReadOnlyList<short> years) =>
        await GetStudentYearsRecordsAsync(r => years.Contains(r.Holding) && r.StudentId.SequenceEqual(studentId));

    public async Task<IReadOnlyList<StudentYearsRecordsDto>> GetMadeChoices(byte[] studentId) =>
        await GetStudentYearsRecordsAsync(r => r.StudentId.SequenceEqual(studentId));

    public async Task<IReadOnlyList<RecordWithStudentInfoDto>> GetRecordsWithStudentInfo(uint disciplineId, Semesters semester) =>
        await _context.Records
            .Include(r => r.Student).ThenInclude(s => s.User)
            .Include(r => r.Student).ThenInclude(s => s.FacultyNavigation)
            .Include(r => r.Student).ThenInclude(s => s.GroupNavigation)
            .Where(r => r.DisciplineId == disciplineId && r.Semester == semester)
            .Select(r => new RecordWithStudentInfoDto()
            {
                StudentId = UlidConverter.ByteIdToString(r.StudentId),
                Email = r.Student.User.Email,
                FullName = r.Student.FullName,
                FacultyName = r.Student.FacultyNavigation.FacultyName,
                GroupCode = r.Student.GroupNavigation.GroupCode,
                Semester = r.Semester,
                Approved = r.Approved
            }).ToListAsync();

    public async Task<IReadOnlyList<RecordShortDisciplineInfoDto>> GetWithDisciplineShort(byte[] studentId, short year) =>
        await _context.Records.Where(r => r.StudentId.SequenceEqual(studentId) && r.Holding == year)
            .Select(r => new RecordShortDisciplineInfoDto()
            {
                RecordId = r.RecordId,
                ChosenSemester = r.Semester,
                Approved = r.Approved,
                DisciplineId = r.Discipline.DisciplineId,
                DisciplineCode = r.Discipline.DisciplineCode,
                DisciplineName = r.Discipline.DisciplineName,
                IsYearLong = r.Discipline.IsYearLong,
            }).ToListAsync();

    private async Task<IReadOnlyList<StudentYearsRecordsDto>> GetStudentYearsRecordsAsync(Expression<Func<Record, bool>> predicate) =>
        await _context.Records.Include(r => r.Discipline)
            .Where(predicate)
            .Select(r => new StudentYearsRecordsDto()
            {
                Holding = r.Holding,
                Semester = r.Semester,
                Approved = r.Approved,
                DisciplineCode = r.Discipline.DisciplineCode,
                DisciplineName = r.Discipline.DisciplineName
            }).ToListAsync();

    private IQueryable<RecordWithDisciplineInfoDto> GetQueryableRecordsWithDiscipline(
        Expression<Func<Record, bool>> wherePredicate) =>
        _context.Records
            .Include(r => r.Discipline)
            .Where(wherePredicate)
            .Select(r => new RecordWithDisciplineInfoDto()
            {
                RecordId = r.RecordId,
                ChosenSemester = r.Semester,
                Approved = r.Approved,
                DisciplineId = r.Discipline.DisciplineId,
                DisciplineCode = r.Discipline.DisciplineCode,
                DisciplineName = r.Discipline.DisciplineName,
                Course = CourseMaskConverter.GetCourseMaskString(r.Discipline.Course),
                EduLevel = r.Discipline.EduLevel,
                Semester = r.Discipline.Semester,
                SubscribersCount = _context.Records
                .Count(rs => rs.DisciplineId == r.DisciplineId && rs.Holding == r.Holding && rs.Semester == r.Semester),
                IsYearLong = r.Discipline.IsYearLong,
                IsOpen = r.Discipline.IsOpen,
            });
}