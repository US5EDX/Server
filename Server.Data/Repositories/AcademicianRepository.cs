using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class AcademicianRepository : IAcademicianRepository
    {
        private readonly ElCoursesDbContext _context;

        public AcademicianRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCount(uint facultyId)
        {
            return await _context.Users
                .Include(u => u.Worker)
                .Include(u => u.Student)
                .Where(u => u.Role > 2 && ((u.Worker != null && u.Worker.Faculty == facultyId) ||
                (u.Student != null && u.Student.Faculty == facultyId)))
                .CountAsync();
        }

        public async Task<int> GetCount(uint facultyId, byte roleFilter)
        {
            if (roleFilter == 3)
                return await _context.Users
                    .Include(u => u.Worker)
                    .Where(u => u.Role == roleFilter && u.Worker.Faculty == facultyId)
                    .CountAsync();

            if (roleFilter == 4)
                return await _context.Users
                        .Include(u => u.Student)
                        .Where(u => u.Role == roleFilter && u.Student.Faculty == facultyId)
                        .CountAsync();

            return 0;
        }

        public async Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyId)
        {
            return await GetAllAsQueryable()
                .OrderBy(u => u.UserId)
                .Where(u => u.Role > 2 && ((u.Worker != null && u.Worker.Faculty == facultyId) ||
                (u.Student != null && u.Student.Faculty == facultyId)))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyId, byte roleFilter)
        {
            if (roleFilter == 3)
                return await _context.Users
                    .Include(u => u.Worker).ThenInclude(w => w.FacultyNavigation)
                    .Include(u => u.Worker).ThenInclude(w => w.GroupNavigation)
                    .OrderBy(u => u.UserId)
                    .Where(u => u.Role == roleFilter && u.Worker.Faculty == facultyId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            if (roleFilter == 4)
                return await _context.Users
                    .Include(u => u.Student).ThenInclude(s => s.FacultyNavigation)
                    .Include(u => u.Student).ThenInclude(s => s.GroupNavigation)
                    .OrderBy(u => u.UserId)
                    .Where(u => u.Role == roleFilter && u.Student.Faculty == facultyId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return Enumerable.Empty<User>();
        }

        private IQueryable<User> GetAllAsQueryable()
        {
            return _context.Users
                .Include(u => u.Worker).ThenInclude(w => w.FacultyNavigation)
                .Include(u => u.Worker).ThenInclude(w => w.GroupNavigation)
                .Include(u => u.Student).ThenInclude(s => s.FacultyNavigation)
                .Include(u => u.Student).ThenInclude(s => s.GroupNavigation)
                .AsQueryable();
        }
    }
}
