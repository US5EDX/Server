using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories;

public class HoldingRepository(ElCoursesDbContext context) : IHoldingRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<Holding>> GetAll() => await _context.Holdings.ToListAsync();

    public async Task<short> GetLastYearAsync() =>
        await _context.Holdings.OrderByDescending(h => h.EduYear).Select(h => h.EduYear).FirstAsync();

    public async Task<IReadOnlyList<short>> GetLastNYears(int limit) =>
        await _context.Holdings.OrderByDescending(h => h.EduYear).Take(limit).Select(h => h.EduYear).ToListAsync();

    public async Task<Holding> GetLast() =>
        await _context.Holdings.OrderByDescending(h => h.EduYear).FirstAsync();

    public async Task<IReadOnlyList<short>> GetRequestedYears(HashSet<int> requestedYears) =>
        await _context.Holdings.Where(h => requestedYears.Contains(h.EduYear)).Select(h => h.EduYear).ToListAsync();

    public async Task<Holding> Add(Holding holding)
    {
        await _context.Holdings.AddAsync(holding);
        await _context.SaveChangesAsync();

        return holding;
    }

    public async Task<Holding?> Update(Holding holding)
    {
        var existingHolding = await _context.Holdings.SingleOrDefaultAsync(h => h.EduYear == holding.EduYear);

        if (existingHolding is null) return null;

        existingHolding.StartDate = holding.StartDate;
        existingHolding.EndDate = holding.EndDate;
        await _context.SaveChangesAsync();

        return existingHolding;
    }

    public async Task<DeleteResultEnum> Delete(short eduYear)
    {
        bool hasDependencies = await _context.Holdings
                .Where(h => h.EduYear == eduYear)
                .AnyAsync(h => h.Disciplines.Any() || h.Records.Any());

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        var existingHolding = await _context.Holdings.SingleOrDefaultAsync(h => h.EduYear == eduYear);

        if (existingHolding is null) return DeleteResultEnum.ValueNotFound;

        _context.Holdings.Remove(existingHolding);
        await _context.SaveChangesAsync();

        return DeleteResultEnum.Success;
    }
}
