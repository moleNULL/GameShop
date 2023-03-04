using Basket.WebAPI.Configurations;
using Basket.WebAPI.Services.Implementations;
using Basket.WebAPI.Services.Interfaces;

namespace Basket.UnitTests.Services
{
    public class CacheServiceTest
    {
        private readonly ICacheService _cacheService;

        private readonly Mock<IOptions<BasketRedisConfig>> _basketRedisConfig;
        private readonly Mock<ILogger<CacheService>> _logger;
        private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionService;

        private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer;
        private readonly Mock<IDatabase> _redisDb;

        public CacheServiceTest()
        {
            _logger = new Mock<ILogger<CacheService>>();

            _basketRedisConfig = new Mock<IOptions<BasketRedisConfig>>();
            _basketRedisConfig.Setup(x => x.Value).Returns(new BasketRedisConfig() { CacheTimeout = TimeSpan.Zero });

            _redisCacheConnectionService = new Mock<IRedisCacheConnectionService>();
            _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _redisDb = new Mock<IDatabase>();

            _connectionMultiplexer
                .Setup(x => x.GetDatabase(
                    It.IsAny<int>(),
                    It.IsAny<object>()))
                .Returns(_redisDb.Object);

            _redisCacheConnectionService
                .Setup(x => x.Connection)
                .Returns(_connectionMultiplexer.Object);

            _cacheService =
                new CacheService(
                    _logger.Object,
                    _redisCacheConnectionService.Object,
                    _basketRedisConfig.Object);
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            // arrange
            string data = "\"data\"";
            string expectedResult = "data";

            _redisDb.Setup(x => x.StringGetAsync(
                    It.IsAny<RedisKey>(),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(data);

            // act
            string actualResult = await _cacheService.GetAsync<string>(data);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task GetAsync_Failed()
        {
            // arrange
            string data = "data";

            // act
            string actualResult = await _cacheService.GetAsync<string>(data);

            // assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public async Task SetAsync_Success()
        {
            // arrange
            var testObject = new
            {
                Key = "basketId",
                Value = "some_data"
            };

            _redisDb.Setup(x => x.StringSetAsync(
                    It.IsAny<RedisKey>(),
                    It.IsAny<RedisValue>(),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            // act
            bool actualResult = await _cacheService.SetAsync(
                testObject.Key,
                testObject.Value);

            // assert
            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task SetAsync_Failed()
        {
            // arrange
            var testObject = new
            {
                Key = "bId",
                Value = "nothing"
            };

            _redisDb.Setup(x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(false);

            // act
            bool actualResult = await _cacheService.SetAsync(testObject.Key, testObject.Value);

            // assert
            actualResult.Should().BeFalse();
        }
    }
}
