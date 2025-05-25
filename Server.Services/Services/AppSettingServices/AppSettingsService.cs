using Server.Models.CustomExceptions;
using Server.Models.Interfaces;
using Server.Services.Dtos.SettingDtos;
using Server.Services.Stores;
using System.Text.Json;

namespace Server.Services.Services.AppSettingServices;

public class AppSettingsService<T>(IAppSettingRepository appSettingRepository, IAppSettingStore<T> store) :
    IAppSettingsService<T>
{
    private string Key => typeof(T).Name switch
    {
        nameof(Thresholds) => "DisciplineStatusThresholds",
        _ => throw new InvalidDataException("Unknown type")
    };

    public async Task<T> Get()
    {
        if (store.Value is not null)
            return store.Value;

        string jsonValue = await appSettingRepository.GetJsonValue(Key) ??
            throw new NotFoundException("Налаштування не знайдено");

        var value = JsonSerializer.Deserialize<T>(jsonValue) ??
            throw new BadRequestException("Не вдалось отримати налаштування");

        await store.UpdateValue(value);

        return value;
    }

    public async Task UpdateOrThrow(T value)
    {
        var jsonValue = JsonSerializer.Serialize(value);

        var isSuccess = await appSettingRepository.UpdateJsonValue(Key, jsonValue);

        if (!isSuccess) throw new NotFoundException("Налаштування не знайдено");

        await store.UpdateValue(value);
    }
}
