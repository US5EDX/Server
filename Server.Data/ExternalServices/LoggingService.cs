using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Server.Models.Models;
using Server.Services.Options.ContextOptions.RequestContext;
using System.Text.Json;

namespace Server.Data.ExternalServices;

public static class LoggingService
{
    public static Auditlog CreateLog(EntityState state, EntityEntry entityEntry, IRequestContext requestContext)
    {
        var log = new Auditlog()
        {
            UserId = requestContext.UserId ?? "Не ідентифіковано",
            IpAddress = requestContext.IpAddress,
            ActionType = state.ToString(),
            EntityName = entityEntry.Entity.GetType().Name,
            Timestamp = DateTime.UtcNow,
            NewValue = state switch
            {
                EntityState.Added => JsonSerializer.Serialize(entityEntry.Entity),
                _ => null,
            },

            OldValue = state switch
            {
                EntityState.Deleted => JsonSerializer.Serialize(entityEntry.Entity),
                _ => null,
            }
        };

        if (state != EntityState.Modified)
            return log;

        Dictionary<string, object?> oldValues = [];
        Dictionary<string, object?> newValues = [];

        foreach (var prop in entityEntry.Properties.Where(x => !x.IsTemporary))
        {
            if (prop.IsModified && (prop.OriginalValue is null || !prop.OriginalValue.Equals(prop.CurrentValue)))
            {
                var propertyName = prop.Metadata.Name;

                log.Description = ValidateProperty(propertyName, prop.OriginalValue, prop.CurrentValue);

                if (log.Description is not null) break;

                oldValues[propertyName] = prop.OriginalValue;
                newValues[propertyName] = prop.CurrentValue;
            }
        }

        if (oldValues.Count == 0) return log;

        log.OldValue = JsonSerializer.Serialize(oldValues);
        log.NewValue = JsonSerializer.Serialize(newValues);

        return log;
    }

    private static string? ValidateProperty(in string propertyName, in object? originalValue, in object? currentValue) =>
        propertyName switch
        {
            "Password" or "Salt" => "Було оновлено пароль",
            "RefreshToken" or "RefreshTokenExpiry" => originalValue is null ? "Новий вхід в аккаунт за допомогою паролю" :
            currentValue is null ? "Вихід з аккаунту" : "Було оновлено токен оновлення",
            _ => null
        };
}
