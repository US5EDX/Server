using Server.Models.CustomExceptions;
using Server.Services.Dtos.SettingDtos;
using Server.Services.Validations;
using System.Text.Json;

namespace Server.Services.Services.AppSettingServices;

public class KeyedAppSettingsService(IAppSettingsService<Thresholds> thresholdsSerivce)
{
    public async Task<object> GetByKey(string key) => key switch
    {
        "Thresholds" => await thresholdsSerivce.Get(),
        _ => throw new NotFoundException("Налаштування не знайдено")
    };

    public async Task UpdateByKey(string key, JsonElement body)
    {
        switch (key)
        {
            case "Thresholds":
                var thresholds = SettingsValidator<Thresholds>.GetValidatedOrThrow(body);
                await thresholdsSerivce.UpdateOrThrow(thresholds);
                break;
            default:
                throw new NotFoundException("Налаштування не знайдено");
        }
    }
}
