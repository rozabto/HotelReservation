namespace Common
{
    public interface IMemoryService
    {
        T GetValue<T>(string key);

        void SetValue<T>(string key, T value);
    }
}