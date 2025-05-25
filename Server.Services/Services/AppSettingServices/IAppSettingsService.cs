namespace Server.Services.Services.AppSettingServices;

public interface IAppSettingsService<T> : IGetOnlyAppSettingsService<T>
{
    Task UpdateOrThrow(T value);
}