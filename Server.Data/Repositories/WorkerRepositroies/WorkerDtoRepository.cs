using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.WorkerDtos;

namespace Server.Data.Repositories.WorkerRepositroies;

public class WorkerDtoRepository(ElCoursesDbContext context) : IWorkerDtoRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<WorkerShortInfoDto>> GetByFacultyAndFullName(uint faculty, string fullName) =>
        await _context.Workers
            .Where(w => w.Faculty == faculty && w.FullName.StartsWith(fullName))
            .Select(w => new WorkerShortInfoDto(w.WorkerId, w.FullName))
            .ToListAsync();
}