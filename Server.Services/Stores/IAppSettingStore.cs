
namespace Server.Services.Stores
{
    public interface IAppSettingStore<T>
    {
        T? Value { get; }

        Task UpdateValue(T value);
    }
}