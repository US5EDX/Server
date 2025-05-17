using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories.WorkerRepositroies;

public class WorkerRepository(ElCoursesDbContext context) : IWorkerRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<int> GetCount() =>
        await _context.Users.Include(u => u.Worker).Where(u => u.Worker != null).CountAsync();

    public async Task<int> GetCount(uint facultyFilter) =>
        await _context.Users.Include(u => u.Worker).Where(u => u.Worker != null && u.Worker.Faculty == facultyFilter).CountAsync();

    public async Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize) =>
        await GetAsQueryable().Where(u => u.Worker != null).OrderBy(u => u.UserId)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<IEnumerable<User>> GetWorkers(int pageNumber, int pageSize, uint facultyFilter) =>
        await GetAsQueryable().Where(u => u.Worker != null && u.Worker.Faculty == facultyFilter).OrderBy(u => u.UserId)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<User> Add(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.Users.AddAsync(user);
        await _context.Workers.AddAsync(user.Worker);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return await GetAsQueryable().FirstAsync(u => u.UserId.SequenceEqual(user.UserId));
    }

    public async Task<User?> Update(User user)
    {
        var existingUser = await GetAsQueryable().SingleOrDefaultAsync(u => u.UserId.SequenceEqual(user.UserId));

        if (existingUser is null || existingUser.Worker is null) return null;

        if (existingUser.Email != user.Email)
        {
            existingUser.RefreshToken = null;
            existingUser.RefreshTokenExpiry = null;
        }

        existingUser.Email = user.Email;
        existingUser.Role = user.Role;
        existingUser.Worker.FullName = user.Worker.FullName;
        existingUser.Worker.Faculty = user.Worker.Faculty;
        existingUser.Worker.Department = user.Worker.Department;
        existingUser.Worker.Position = user.Worker.Position;

        await _context.SaveChangesAsync();

        return await GetAsQueryable().FirstAsync(u => u.UserId.SequenceEqual(existingUser.UserId));
    }

    public async Task<DeleteResultEnum> Delete(byte[] userId, Roles requestUserRole, short deleteBorderYear)
    {
        var existingUser = await _context.Users.Include(u => u.Worker).SingleOrDefaultAsync(u => u.UserId.SequenceEqual(userId));

        if (existingUser is null || existingUser.Worker is null || existingUser.Role <= requestUserRole)
            return DeleteResultEnum.ValueNotFound;

        bool hasDependencies =
            await _context.Disciplines.AnyAsync(d => d.CreatorId.SequenceEqual(existingUser.UserId) && d.Holding > deleteBorderYear);

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        _context.Users.Remove(existingUser);
        await _context.SaveChangesAsync();

        return DeleteResultEnum.Success;
    }

    private IQueryable<User> GetAsQueryable() =>
        _context.Users.Include(u => u.Worker).ThenInclude(w => w.FacultyNavigation).AsQueryable();
}
