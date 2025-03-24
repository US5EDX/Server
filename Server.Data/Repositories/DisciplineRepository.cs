using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly ElCoursesDbContext _context;

        public DisciplineRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCount(uint facultyId, short eduYear)
        {
            return await _context.Disciplines
                .Where(d => d.FacultyId == facultyId && d.Holding == eduYear)
                .CountAsync();
        }

        public async Task<int> GetCount(uint facultyId, short eduYear, byte catalogType)
        {
            return await _context.Disciplines
                .Where(d => d.FacultyId == facultyId && d.Holding == eduYear && d.CatalogType == catalogType)
                .CountAsync();
        }

        public async Task<Discipline?> GetById(uint disciplineId)
        {
            return await _context.Disciplines
                .Include(d => d.Faculty)
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(g => g.DisciplineId == disciplineId);
        }

        public async Task<List<Discipline>> GetShortInfoByCodeEduYearEduLevelSemester(string code, short eduYear, byte eduLevel, byte semester)
        {
            return await _context.Disciplines
                .Where(d => d.Holding == eduYear && d.EduLevel == eduLevel &&
                (d.Semester == 0 || d.Semester == semester) && d.DisciplineCode.StartsWith(code))
                .ToListAsync();
        }

        public async Task<Discipline> Add(Discipline discipline)
        {
            await _context.Disciplines.AddAsync(discipline);
            await _context.SaveChangesAsync();

            return await GetById(discipline.DisciplineId);
        }

        public async Task<Discipline?> Update(Discipline discipline)
        {
            var existingDiscipline = await _context.Disciplines.FirstOrDefaultAsync(d => d.DisciplineId == discipline.DisciplineId);

            if (existingDiscipline is null)
                return null;

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

            await _context.SaveChangesAsync();

            return await GetById(existingDiscipline.DisciplineId);
        }

        public async Task<bool> UpdateStatus(uint disciplineId)
        {
            var existingDiscipline = await _context.Disciplines.FindAsync(disciplineId);

            if (existingDiscipline is null)
                return false;

            existingDiscipline.IsOpen = !existingDiscipline.IsOpen;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool?> Delete(uint disciplineId)
        {
            bool hasDependencies = await _context.Disciplines
                    .Where(d => d.DisciplineId == disciplineId && d.IsOpen)
                    .AnyAsync(d => d.Records.Any());

            if (hasDependencies)
                return null;

            var existingDiscipline = await _context.Disciplines.FindAsync(disciplineId);

            if (existingDiscipline is null)
                return false;

            _context.Disciplines.Remove(existingDiscipline);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
