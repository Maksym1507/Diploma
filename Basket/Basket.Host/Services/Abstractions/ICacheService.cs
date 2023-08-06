namespace Basket.Host.Services.Abstractions
{
    public interface ICacheService
    {
        Task AddOrUpdateAsync<T>(string key, T value);

        Task<T> GetAsync<T>(string key);

        Task<bool> RemoveAsync(string key);
    }
}
