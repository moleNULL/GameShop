using System.Text.Json;
using Basket.WebAPI.Configurations;
using Basket.WebAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Basket.WebAPI.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IRedisCacheConnectionService _redisCacheConnectionService;
        private readonly BasketRedisConfig _basketRedisConfig;

        public CacheService(
            ILogger<CacheService> logger,
            IRedisCacheConnectionService redisCacheConnectionService,
            IOptions<BasketRedisConfig> basketRedisConfig)
        {
            _logger = logger;
            _redisCacheConnectionService = redisCacheConnectionService;
            _basketRedisConfig = basketRedisConfig.Value;
        }

        public async Task<bool> SetAsync<T>(string key, T value, IDatabase redisDb = null!, TimeSpan? expiry = null)
        {
            redisDb ??= GetRedisDatabase();
            expiry ??= _basketRedisConfig.CacheTimeout;

            string cacheKey = GetItemCacheKey(key);
            var serialized = JsonSerializer.Serialize(value);

            bool isSet = await redisDb.StringSetAsync(cacheKey, serialized, expiry);

            if (isSet)
            {
                _logger.LogInformation($"Cached value for {key} was added to cache");
            }
            else
            {
                _logger.LogInformation($"Failed to set the cached value for {key}");
            }

            return isSet;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var redisDb = GetRedisDatabase();
            string cacheKey = GetItemCacheKey(key);
            RedisValue serialized = await redisDb.StringGetAsync(cacheKey);

            return serialized.HasValue ? JsonSerializer.Deserialize<T>(serialized.ToString()) ! : default !;
        }

        public async Task<bool> FlushAsync(string key)
        {
            var redisDb = GetRedisDatabase();
            string cacheKey = GetItemCacheKey(key);

            bool isFlushed = await redisDb.KeyDeleteAsync(cacheKey);
            return isFlushed;
        }

        private IDatabase GetRedisDatabase()
        {
            return _redisCacheConnectionService.Connection.GetDatabase();
        }

        private static string GetItemCacheKey(string userId)
        {
            return $"{userId}";
        }
    }
}
