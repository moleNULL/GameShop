using Basket.WebAPI.Configurations;
using Basket.WebAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Basket.WebAPI.Services.Implementations
{
    public class RedisCacheConnectionService : IRedisCacheConnectionService, IDisposable
    {
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexerLazy;
        private bool _disposed;

        public RedisCacheConnectionService(IOptions<BasketRedisConfig> basketRedisConfig)
        {
            var redisHost = ConfigurationOptions.Parse(basketRedisConfig.Value.Host);
            _connectionMultiplexerLazy = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(redisHost);
            });
        }

        public IConnectionMultiplexer Connection => _connectionMultiplexerLazy.Value;

        public void Dispose()
        {
            if (!_disposed)
            {
                Connection.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
