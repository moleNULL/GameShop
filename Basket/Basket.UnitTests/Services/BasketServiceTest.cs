using Basket.WebAPI.Configurations;
using Basket.WebAPI.Models;
using Basket.WebAPI.Services.Implementations;
using Basket.WebAPI.Services.Interfaces;
using Infrastructure.Exceptions;

namespace Basket.UnitTests.Services
{
    public class BasketServiceTest
    {
        private readonly IBasketService _basketService;

        private readonly Mock<ICacheService> _cacheService;
        private readonly Mock<IOptions<BasketRedisConfig>> _basketRedisConfig;
        private readonly Mock<ILogger<CacheService>> _logger;
        private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionService;
        private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer;
        private readonly Mock<IDatabase> _redisDb;

        public BasketServiceTest()
        {
            _logger = new Mock<ILogger<CacheService>>();

            _basketRedisConfig = new Mock<IOptions<BasketRedisConfig>>();
            _basketRedisConfig.Setup(x => x.Value).Returns(new BasketRedisConfig() { CacheTimeout = TimeSpan.Zero });

            _redisCacheConnectionService = new Mock<IRedisCacheConnectionService>();
            _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _redisDb = new Mock<IDatabase>();

            _connectionMultiplexer.Setup(x => x.GetDatabase(
                    It.IsAny<int>(),
                    It.IsAny<object>()))
                .Returns(_redisDb.Object);

            _redisCacheConnectionService.Setup(x => x.Connection).Returns(_connectionMultiplexer.Object);

            _cacheService = new Mock<ICacheService>();

            _basketService = new BasketService(_cacheService.Object);
        }

        [Fact]
        public async Task GetItemsAsync_Success()
        {
            // arrange
            string? userId = "123";
            var expectedServiceResult = new List<ItemDto>();

            _cacheService.Setup(x => x.GetAsync<List<ItemDto>>(It.Is<string>(i => i == userId)))
                .ReturnsAsync(expectedServiceResult);

            // act
            var actualResult = await _basketService.GetItemsAsync(userId);

            // arrange
            actualResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetItemsAsync_Failed()
        {
            // arrange
            string? userId = "-123";
            List<ItemDto> items = null!;
            var expectedResult = new GetItemsResponse { Items = null! };

            _cacheService.Setup(x => x.GetAsync<List<ItemDto>>(It.Is<string>(i => i == userId)))
                .ReturnsAsync(items);

            // act
            var actualResult = await _basketService.GetItemsAsync(userId);

            // assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetItemsAsync_ThrowsException()
        {
            // arrange
            string? userId = null;

            // act
            var act = async () => await _basketService.GetItemsAsync(userId);

            // assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Cannot get items because userId is null");
        }

        [Fact]
        public async Task SetItemsAsync_Success()
        {
            // arrange
            string? userId = "123";
            var items = new List<ItemDto>() { new ItemDto { ItemId = 6, ItemName = "Europa III Universalis" } };

            _cacheService.Setup(x => x.SetAsync(
                It.Is<string>(i => i == userId),
                It.Is<List<ItemDto>>(i => i == items),
                It.IsAny<IDatabase>(),
                It.IsAny<TimeSpan?>()))
            .ReturnsAsync(true);

            // act
            bool actualResult = await _basketService.SetItemsAsync(userId, items);

            // assert
            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task SetItemsAsync_Failed()
        {
            // arrange
            string? userId = "-123";
            List<ItemDto> items = null!;

            _cacheService.Setup(x => x.SetAsync(
                It.Is<string>(i => i == userId),
                It.Is<List<ItemDto>>(i => i == items),
                It.IsAny<IDatabase>(),
                It.IsAny<TimeSpan?>()))
            .ReturnsAsync(false);

            // act
            bool actualResult = await _basketService.SetItemsAsync(userId, items);

            // assert
            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task SetItemsAsync_ThrowsException()
        {
            // arrange
            string? userId = null;
            List<ItemDto> items = null!;

            // act
            var act = async () => await _basketService.SetItemsAsync(userId, items);

            // assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Cannot set items because userId is null");
        }

        [Fact]
        public async Task DeleteItemsAsync_Success()
        {
            // arrange
            string? userId = "123";

            _cacheService.Setup(x => x.DeleteItemsAsync(It.Is<string>(i => i == userId))).ReturnsAsync(true);

            // act
            bool actualResult = await _basketService.DeleteItemsAsync(userId);

            // arrange
            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteItemsAsync_Failed()
        {
            // arrange
            string? userId = "-123";
            _cacheService.Setup(x => x.DeleteItemsAsync(It.Is<string>(i => i == userId))).ReturnsAsync(false);

            // act
            bool actualResult = await _basketService.DeleteItemsAsync(userId);

            // arrange
            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task DeletedItemsAsync_ThrowsException()
        {
            // arrange
            string? userId = null;

            // act
            var act = async () => await _basketService.DeleteItemsAsync(userId);

            // arrange
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Cannot delete data because userId is null");
        }
    }
}
