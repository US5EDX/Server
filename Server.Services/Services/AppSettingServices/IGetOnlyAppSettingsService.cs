namespace Server.Services.Services.AppSettingServices;

public interface IGetOnlyAppSettingsService<T>
{
    Task<T> Get();
}