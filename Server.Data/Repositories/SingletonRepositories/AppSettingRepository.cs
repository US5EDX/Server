using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;

namespace Server.Data.Repositories.SingletonRepositories;

public class AppSettingRepository(ElCoursesDbContext context) : IAppSettingRepository
{
    public async Task<string?> GetJsonValue(string key) =>
        await context.AppSettings
        .Where(x => x.Key == key)
        .Select(x => x.JsonValue)
        .SingleOrDefaultAsync();

    public async Task<bool> UpdateJsonValue(string key, string jsonValue)
    {
        var setting = await context.AppSettings.SingleOrDefaultAsync(x => x.Key == key);

        if (setting is null) return false;

        setting.JsonValue = jsonValue;

        await context.SaveChangesAsync();

        return true;
    }
}
