using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories.WorkerRepositroies;

public class AcademicianRepository(ElCoursesDbContext context) : IAcademicianRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<int> GetCount(uint facultyId, Roles? roleFilter) =>
        await ApplyFilter(_context.Users.AsQueryable(), roleFilter, facultyId).CountAsync();

    public async Task<IReadOnlyList<User>> GetAcademicians(int pageNumber, int pageSize, uint facultyId, Roles? roleFilter)
    {
        var query = _context.Users.AsQueryable();

        if (roleFilter is null || roleFilter == Roles.Lecturer)
            query = query.Include(u => u.Worker).ThenInclude(w => w.FacultyNavigation);

        if (roleFilter is null || roleFilter == Roles.Student)
            query = query.Include(u => u.Student).ThenInclude(s => s.FacultyNavigation)
            .Include(u => u.Student).ThenInclude(s => s.GroupNavigation);

        query = ApplyFilter(query, roleFilter, facultyId);

        return await query
            .OrderBy(u => u.UserId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    private IQueryable<User> ApplyFilter(IQueryable<User> query, Roles? roleFilter, uint facultyId) =>
        roleFilter switch
        {
            null => query.Where(u => u.Role > Roles.Admin && (u.Worker != null && u.Worker.Faculty == facultyId ||
            u.Student != null && u.Student.Faculty == facultyId)),
            Roles.Lecturer => query.Where(u => u.Role == roleFilter && u.Worker.Faculty == facultyId),
            Roles.Student => query.Where(u => u.Role == roleFilter && u.Student.Faculty == facultyId),
            _ => throw new InvalidOperationException("Role filter invalid")
        };
}
