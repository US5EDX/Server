namespace Server.Models.Interfaces
{
    public interface IAppSettingRepository
    {
        Task<string?> GetJsonValue(string key);
        Task<bool> UpdateJsonValue(string key, string jsonValue);
    }
}