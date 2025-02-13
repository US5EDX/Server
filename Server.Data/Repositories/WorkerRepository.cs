using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly ElCoursesDbContext _context;

        public WorkerRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCount()
        {
            return await _context.Users
                .Include(u => u.Worker)
                .Where(u => u.Worker != null)
                .CountAsync();
        }

        public async Task<int> GetCount(uint facultyFilter)
        {
            return await _context.Users
                .Include(u => u.Worker)
                .Where(u => u.Worker != null && u.Worker.Faculty == facultyFilter)
                .CountAsync();
        }

        public async Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize)
        {
            return await GetAllAsQueryable()
                .OrderBy(u => u.UserId)
                .Where(u => u.Worker != null)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyFilter)
        {
            return await GetAllAsQueryable()
                .OrderBy(u => u.UserId)
                .Where(u => u.Worker != null && u.Worker.Faculty == facultyFilter)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<User> Add(User user)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.Users.AddAsync(user);

            if (user.Worker != null)
            {
                await _context.Workers.AddAsync(user.Worker);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetAllAsQueryable().FirstAsync(u => u.UserId.SequenceEqual(user.UserId));
        }

        public async Task<User?> Update(User user)
        {
            var existingUser = await GetAllAsQueryable().FirstOrDefaultAsync(u => u.UserId.SequenceEqual(user.UserId));

            if (existingUser is null)
                return null;

            if (existingUser.Email != user.Email)
            {
                existingUser.RefreshToken = null;
                existingUser.RefreshTokenExpiry = null;
            }

            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            if (existingUser.Worker is not null)
            {
                existingUser.Worker.FullName = user.Worker.FullName;
                existingUser.Worker.Faculty = user.Worker.Faculty;
                existingUser.Worker.Department = user.Worker.Department;
                existingUser.Worker.Position = user.Worker.Position;
                existingUser.Worker.Group = user.Worker.Group;
            }

            await _context.SaveChangesAsync();

            return await GetAllAsQueryable().FirstAsync(u => u.UserId.SequenceEqual(existingUser.UserId));
        }

        public async Task<bool?> Delete(byte[] userId)
        {
            var existingUser = await _context.Users
                .Include(u => u.Worker)
                .FirstOrDefaultAsync(u => u.UserId.SequenceEqual(userId));

            if (existingUser is null || existingUser.Worker is null)
                return false;

            bool hasDependencies =
                await _context.Disciplines.AnyAsync(d => d.CreatorId.SequenceEqual(existingUser.Worker.WorkerId));

            if (hasDependencies)
                return null;

            if (existingUser.Worker is not null)
            {
                _context.Workers.Remove(existingUser.Worker);
            }

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();

            return true;
        }

        private IQueryable<User> GetAllAsQueryable()
        {
            return _context.Users
                .Include(u => u.Worker).ThenInclude(w => w.FacultyNavigation)
                .Include(u => u.Worker).ThenInclude(w => w.GroupNavigation)
                .AsQueryable();
        }
    }
}
