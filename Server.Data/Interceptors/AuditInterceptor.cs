using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Server.Data.ExternalServices;
using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.Options.ContextOptions.RequestContext;

namespace Server.Data.Interceptors;

public class AuditInterceptor(IRequestContext requestContext,
    [FromKeyedServices("Audit")] List<(Auditlog Audit, Lazy<object?> Pk)> logs) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker.Entries()
        .Where(e => e.Entity is not Auditlog && (e.State is EntityState.Modified ||
        (e.State is EntityState.Added or EntityState.Deleted && e.Entity is not User)))
        .Select(entry => (LoggingService.CreateLog(entry.State, entry, requestContext),
        new Lazy<object?>(() => entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue)))
        .ToList();

        if (entries.Count > 0)
            logs.AddRange(entries);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null || logs.Count == 0)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        foreach (var (log, Pk) in logs)
        {
            var primaryKey = Pk.Value;

            log.EntityId = primaryKey is null ? "Не визначено" :
                primaryKey is byte[] userId ? UlidConverter.ByteIdToString(userId) : primaryKey?.ToString();

            eventData.Context.Set<Auditlog>().Add(log);
        }

        logs.Clear();
        await eventData.Context.SaveChangesAsync(cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
