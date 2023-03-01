namespace Basket.WebAPI.Configurations
{
    public class BasketRedisConfig
    {
        public string Host { get; set; } = null!;
        public TimeSpan CacheTimeout { get; set; }
    }
}
