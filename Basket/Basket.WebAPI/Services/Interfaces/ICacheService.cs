using StackExchange.Redis;

namespace Basket.WebAPI.Services.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, IDatabase redisDb = null!, TimeSpan? expiry = null);
        Task<T> GetAsync<T>(string key);
    }
}
