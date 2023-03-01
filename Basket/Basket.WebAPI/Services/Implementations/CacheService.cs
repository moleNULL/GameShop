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

        public async Task SetAsync<T>(string key, T value, IDatabase redisDb = null!, TimeSpan? expiry = null)
        {
            redisDb ??= GetRedisDatabase();
            expiry ??= _basketRedisConfig.CacheTimeout;

            string cacheKey = GetItemCacheKey(key);
            RedisValue serialized = JsonSerializer.Serialize(value);

            bool isAdded = await redisDb.StringSetAsync(cacheKey, serialized, expiry);

            if (isAdded)
            {
                _logger.LogInformation($"Cached value for {key} was added to cache");
            }
            else
            {
                _logger.LogInformation($"Failed to set the cached value for {key}");
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var redisDb = GetRedisDatabase();
            string cacheKey = GetItemCacheKey(key);
            RedisValue serialized = await redisDb.StringGetAsync(cacheKey);

            return serialized.HasValue ? JsonSerializer.Deserialize<T>(serialized.ToString()) ! : default !;
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
