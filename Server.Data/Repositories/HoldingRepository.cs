using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class HoldingRepository : IHoldingRepository
    {
        private readonly ElCoursesDbContext _context;

        public HoldingRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Holding>> GetAll()
        {
            return await _context.Holdings.ToListAsync();
        }

        public async Task<IEnumerable<short>> GetLastFive()
        {
            return await _context.Holdings
                .OrderByDescending(h => h.EduYear)
                .Take(5)
                .Select(h => h.EduYear)
                .ToListAsync();
        }

        public async Task<Holding> Add(Holding holding)
        {
            await _context.Holdings.AddAsync(holding);
            await _context.SaveChangesAsync();

            return holding;
        }

        public async Task<Holding?> Update(Holding holding)
        {
            var existingHolding = await _context.Holdings.FirstOrDefaultAsync(h => h.EduYear == holding.EduYear);

            if (existingHolding is null)
                return null;

            existingHolding.StartDate = holding.StartDate;
            existingHolding.EndDate = holding.EndDate;
            await _context.SaveChangesAsync();

            return existingHolding;
        }

        public async Task<bool?> Delete(short eduYear)
        {
            bool hasDependencies = await _context.Holdings
                    .Where(h => h.EduYear == eduYear)
                    .AnyAsync(h =>
                        h.Disciplines.Any() ||
                        h.Records.Any());

            if (hasDependencies)
                return null;

            var existingHolding = await _context.Holdings.FirstOrDefaultAsync(h => h.EduYear == eduYear);

            if (existingHolding is null)
                return false;

            _context.Holdings.Remove(existingHolding);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
