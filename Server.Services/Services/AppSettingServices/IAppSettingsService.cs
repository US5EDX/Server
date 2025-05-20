namespace Server.Services.Services.AppSettingServices;

public interface IAppSettingsService<T>
{
    Task<T> Get();
    Task UpdateOrThrow(T thresholds);
}