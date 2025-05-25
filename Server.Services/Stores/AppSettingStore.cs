using Microsoft.Extensions.DependencyInjection;
using Server.Services.Dtos.SettingDtos;

namespace Server.Services.Stores;

public class AppSettingStore<T>([FromKeyedServices(nameof(Thresholds))] SemaphoreSlim semaphore) : IAppSettingStore<T>
{
    public T? Value { get; private set; }

    public async Task UpdateValue(T value)
    {
        await semaphore.WaitAsync();

        Value = value;

        semaphore.Release();
    }
}
