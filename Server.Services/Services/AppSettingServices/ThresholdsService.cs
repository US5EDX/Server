using Microsoft.Extensions.DependencyInjection;
using Server.Models.CustomExceptions;
using Server.Models.Interfaces;
using Server.Services.Dtos.SettingDtos;
using System.Text.Json;

namespace Server.Services.Services.AppSettingServices;

public class ThresholdsService(IServiceScopeFactory scopeFactory,
    [FromKeyedServices("ThresholdsLock")] SemaphoreSlim semaphore) : IAppSettingsService<Thresholds>
{
    private Thresholds? _cached;

    public async Task<Thresholds> Get()
    {
        if (_cached is not null)
            return _cached;

        using var scope = scopeFactory.CreateScope();
        var appSettingRepository = scope.ServiceProvider.GetRequiredService<IAppSettingRepository>();

        string jsonValue = await appSettingRepository.GetJsonValue("DisciplineStatusThresholds") ??
            throw new NotFoundException("Налаштування не знайдено");

        await UseSemaphore(() => _cached ??= JsonSerializer.Deserialize<Thresholds>(jsonValue) ??
                throw new BadRequestException("Не вдалось отримати налаштування"));

        return _cached;
    }

    public async Task UpdateOrThrow(Thresholds thresholds)
    {
        var jsonValue = JsonSerializer.Serialize(thresholds);

        using var scope = scopeFactory.CreateScope();
        var appSettingRepository = scope.ServiceProvider.GetRequiredService<IAppSettingRepository>();

        var isSuccess = await appSettingRepository.UpdateJsonValue("DisciplineStatusThresholds", jsonValue);

        if (!isSuccess) throw new NotFoundException("Налаштування не знайдено");

        await UseSemaphore(() => _cached = thresholds);
    }

    private async Task UseSemaphore(Action action)
    {
        await semaphore.WaitAsync(1000);

        try
        {
            action();
        }
        finally
        {
            semaphore.Release();
        }
    }
}
