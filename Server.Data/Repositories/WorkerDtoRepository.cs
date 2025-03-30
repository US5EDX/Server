using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;

namespace Server.Data.Repositories
{
    public class WorkerDtoRepository : IWorkerDtoRepository
    {
        private readonly ElCoursesDbContext _context;

        public WorkerDtoRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkerShortInfoDto>> GetByFacultyAndFullName(uint faculty, string fullName)
        {
            return await _context.Workers
                .Where(w => w.Faculty == faculty && w.FullName.StartsWith(fullName))
                .Select(w => new WorkerShortInfoDto(w.WorkerId, w.FullName))
                .ToListAsync();
        }
    }
}
