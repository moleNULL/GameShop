using StackExchange.Redis;

namespace Basket.WebAPI.Services.Interfaces
{
    public interface IRedisCacheConnectionService
    {
        IConnectionMultiplexer Connection { get; }
    }
}
