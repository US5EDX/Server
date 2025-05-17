using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories;

public class FacultyRepository(ElCoursesDbContext context) : IFacultyRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<Faculty>> GetAll() => await _context.Faculties.ToListAsync();

    public async Task<Faculty> Add(Faculty faculty)
    {
        await _context.Faculties.AddAsync(faculty);
        await _context.SaveChangesAsync();

        return faculty;
    }

    public async Task<int> Update(Faculty faculty) =>
        await _context.Faculties.Where(f => f.FacultyId == faculty.FacultyId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(f => f.FacultyName, faculty.FacultyName));

    public async Task<DeleteResultEnum> Delete(uint facultyId)
    {
        bool hasDependencies = await _context.Faculties
                .Where(f => f.FacultyId == facultyId)
                .AnyAsync(f =>
                    f.Disciplines.Any() ||
                    f.Specialties.Any() ||
                    f.Students.Any() ||
                    f.Workers.Any());

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        var existingFaculty = await _context.Faculties.SingleOrDefaultAsync(f => f.FacultyId == facultyId);

        if (existingFaculty is null) return DeleteResultEnum.ValueNotFound;

        _context.Faculties.Remove(existingFaculty);
        await _context.SaveChangesAsync();

        return DeleteResultEnum.Success;
    }
}
