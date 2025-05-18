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

    public async Task<bool> Update(Faculty faculty)
    {
        var existingFaculty = await _context.Faculties.SingleOrDefaultAsync(f => f.FacultyId == faculty.FacultyId);

        if (existingFaculty is null) return false;

        existingFaculty.FacultyName = faculty.FacultyName;
        await _context.SaveChangesAsync();

        return true;
    }

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
