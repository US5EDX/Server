using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories.DisciplineRepositories;

public class DisciplineRepository(ElCoursesDbContext context) : IDisciplineRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<int> GetCount(uint facultyId, short eduYear,
        CatalogTypes? catalogType, Semesters? semesterFilter, byte[]? creatorFilter)
    {
        var query = _context.Disciplines
            .Where(d => d.FacultyId == facultyId && d.Holding == eduYear);

        if (catalogType is not null)
            query = query.Where(d => d.CatalogType == catalogType);

        if (semesterFilter is not null)
            query = query.Where(d => d.Semester == Semesters.Both || d.Semester == semesterFilter);

        if (creatorFilter is not null)
            query = query.Where(d => d.CreatorId.SequenceEqual(creatorFilter));

        return await query.CountAsync();
    }

    public async Task<int> GetCountForStudent(EduLevels eduLevel, short holding,
        CatalogTypes catalogFilter, byte courseMask, Semesters semesterFilter, uint? facultyFilter)
    {
        var query = _context.Disciplines
            .Where(d => d.EduLevel == eduLevel && d.CatalogType == catalogFilter && d.IsOpen
            && (d.Course & courseMask) > 0 && d.Holding == holding);

        if (semesterFilter != Semesters.Both)
            query = query.Where(d => d.Semester == Semesters.Both || d.Semester == semesterFilter);

        if (facultyFilter is not null)
            query = query.Where(d => d.FacultyId == facultyFilter);

        return await query.CountAsync();
    }

    public async Task<Discipline?> GetById(uint disciplineId) =>
        await _context.Disciplines
            .Include(d => d.Faculty)
            .Include(d => d.Specialty)
            .SingleOrDefaultAsync(d => d.DisciplineId == disciplineId);

    public async Task<Discipline> Add(Discipline discipline)
    {
        await _context.Disciplines.AddAsync(discipline);
        await _context.SaveChangesAsync();

        return await GetById(discipline.DisciplineId);
    }

    public async Task<Discipline?> Update(Discipline discipline)
    {
        var existingDiscipline = await _context.Disciplines.SingleOrDefaultAsync(d => d.DisciplineId == discipline.DisciplineId);

        if (existingDiscipline is null) return null;

        existingDiscipline.DisciplineCode = discipline.DisciplineCode;
        existingDiscipline.DisciplineName = discipline.DisciplineName;
        existingDiscipline.CatalogType = discipline.CatalogType;
        existingDiscipline.SpecialtyId = discipline.SpecialtyId;
        existingDiscipline.EduLevel = discipline.EduLevel;
        existingDiscipline.Course = discipline.Course;
        existingDiscipline.Semester = discipline.Semester;
        existingDiscipline.Prerequisites = discipline.Prerequisites;
        existingDiscipline.Interest = discipline.Interest;
        existingDiscipline.MaxCount = discipline.MaxCount;
        existingDiscipline.MinCount = discipline.MinCount;
        existingDiscipline.Url = discipline.Url;
        existingDiscipline.IsYearLong = discipline.IsYearLong;

        await _context.SaveChangesAsync();

        return await GetById(existingDiscipline.DisciplineId);
    }

    public async Task<bool> UpdateStatus(uint disciplineId)
    {
        var existingDiscipline = await _context.Disciplines.FindAsync(disciplineId);

        if (existingDiscipline is null) return false;

        existingDiscipline.IsOpen = !existingDiscipline.IsOpen;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<DeleteResultEnum> Delete(uint disciplineId, int notEnough)
    {
        bool hasDependencies = await _context.Disciplines
                .Where(d => d.DisciplineId == disciplineId)
                .AnyAsync(d => d.Records.Count(r => r.Semester == Semesters.Fall) >= notEnough ||
                 d.Records.Count(r => r.Semester == Semesters.Spring) >= notEnough);

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        var existingDiscipline = await _context.Disciplines.FindAsync(disciplineId);

        if (existingDiscipline is null) return DeleteResultEnum.ValueNotFound;

        _context.Disciplines.Remove(existingDiscipline);
        await _context.SaveChangesAsync();

        return DeleteResultEnum.Success;
    }
}