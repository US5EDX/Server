using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories;

public class AuditLogRepository(ElCoursesDbContext context) : IAuditLogRepository
{
    public async Task<int> GetCount(string actionType, DateTime leftBorder, DateTime rightBorder, bool hasDescription) =>
        await context.Auditlogs
        .CountAsync(a => a.Timestamp.Date >= leftBorder && a.Timestamp.Date <= rightBorder && a.ActionType == actionType &&
        (hasDescription ? a.Description != null : a.Description == null));

    public async Task<IReadOnlyList<Auditlog>> Get(int page, int size,
        string actionType, DateTime leftBorder, DateTime rightBorder, bool hasDescription) =>
        await context.Auditlogs
            .Where(a => a.Timestamp.Date >= leftBorder && a.Timestamp.Date <= rightBorder && a.ActionType == actionType &&
            (hasDescription ? a.Description != null : a.Description == null))
            .OrderByDescending(a => a.Timestamp)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
}
