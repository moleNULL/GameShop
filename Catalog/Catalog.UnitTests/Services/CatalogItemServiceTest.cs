using WebAPI.Models.Dtos;
using WebAPI.Models.Requests;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Implementations;
using WebAPI.Services.Interfaces;

namespace Catalog.UnitTests.Services
{
    public class CatalogItemServiceTest
    {
        private readonly ICatalogItemService _catalogItemService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerBaseData;
        private readonly Mock<ILogger<CatalogItemService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContexWrapper;

        private readonly CreateItemRequest _testItemRequest = new ()
        {
            Name = "NewGame",
            Price = 10,
            Year = 2012,
            AvailableStock = 500,
            PictureFileName = "1.png",
            CatalogCompanyId = 1,
            CatalogGenreId = 1
        };

        private readonly CatalogItemEntity _testItemEntity = new ()
        {
            Id = 1,
            Name = "Test Item",
            Price = 9.99m,
            Year = 2022,
            PictureFileName = "test.jpg",
            AvailableStock = 10,
            CatalogCompanyId = 1,
            CatalogCompany = null!,
            CatalogGenreId = 1,
            CatalogGenre = null!
        };

        private readonly CatalogItemDto _testItemDto = new ()
        {
            Id = 1,
            Name = "Test Item",
            Price = 9.99m,
            Year = 2022,
            PictureUrl = "test.jpg",
            AvailableStock = 10,
            CatalogCompany = null!,
            CatalogGenre = null!
        };

        public CatalogItemServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _loggerBaseData = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _logger = new Mock<ILogger<CatalogItemService>>();
            _mapper = new Mock<IMapper>();
            _dbContexWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContexWrapper.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbContextTransaction.Object);

            _catalogItemService = new CatalogItemService(
                _dbContexWrapper.Object,
                _loggerBaseData.Object,
                _logger.Object,
                _catalogItemRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            int? testResult = 1;

            _catalogItemRepository.Setup(x => x.AddAsync(
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            int? actualResult = await _catalogItemService.AddAsync(
                _testItemRequest.Name,
                _testItemRequest.Price,
                _testItemRequest.Year,
                _testItemRequest.PictureFileName,
                _testItemRequest.AvailableStock,
                _testItemRequest.CatalogCompanyId,
                _testItemRequest.CatalogGenreId);

            // assert
            actualResult.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogItemRepository.Setup(x => x.AddAsync(
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            int? actualResult = await _catalogItemService.AddAsync(
                _testItemRequest.Name,
                _testItemRequest.Price,
                _testItemRequest.Year,
                _testItemRequest.PictureFileName,
                _testItemRequest.AvailableStock,
                _testItemRequest.CatalogCompanyId,
                _testItemRequest.CatalogGenreId);

            // assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            // arrange
            int testId = 1;

            _catalogItemRepository.Setup(x => x.GetByIdAsync(It.Is<int>(i => i == testId)))
                .ReturnsAsync(_testItemEntity);
            _mapper.Setup(x => x.Map<CatalogItemDto>(It.IsAny<CatalogItemEntity>())).Returns(_testItemDto);

            // act
            var actualResult = await _catalogItemService.GetAsync(testId);

            // assert
            actualResult.Should().BeEquivalentTo(_testItemDto);
        }

        [Fact]
        public async Task GetAsync_Failed()
        {
            // arrange
            int testId = 1;

            _catalogItemRepository.Setup(x => x.GetByIdAsync(It.Is<int>(i => i == testId)))
                .ReturnsAsync((CatalogItemEntity)null!);
            _mapper.Setup(x => x.Map<CatalogItemDto>(It.IsAny<CatalogItemEntity>())).Returns((CatalogItemDto)null!);

            // act
            var actualResult = await _catalogItemService.GetAsync(testId);

            // assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            int testId = 1;
            var expectedEntityState = EntityState.Modified;

            _catalogItemRepository.Setup(x => x.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(expectedEntityState);

            // act
            bool actualResult = await _catalogItemService.UpdateAsync(
                testId,
                _testItemRequest.Name,
                _testItemRequest.Price,
                _testItemRequest.Year,
                _testItemRequest.PictureFileName,
                _testItemRequest.AvailableStock,
                _testItemRequest.CatalogCompanyId,
                _testItemRequest.CatalogGenreId);

            // arrange
            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            int testId = 10;
            var expectedEntityState = EntityState.Unchanged;

            _catalogItemRepository.Setup(x => x.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(expectedEntityState);

            // act
            bool actualResult = await _catalogItemService.UpdateAsync(
                testId,
                _testItemRequest.Name,
                _testItemRequest.Price,
                _testItemRequest.Year,
                _testItemRequest.PictureFileName,
                _testItemRequest.AvailableStock,
                _testItemRequest.CatalogCompanyId,
                _testItemRequest.CatalogGenreId);

            // arrange
            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveAsync_Success()
        {
            // arrange
            int testId = 1;
            var expectedEntityState = EntityState.Deleted;

            _catalogItemRepository.Setup(x => x.RemoveAsync(It.IsAny<int>())).ReturnsAsync(expectedEntityState);

            // act
            bool actualResult = await _catalogItemService.RemoveAsync(testId);

            // assert
            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveAsync_Failed()
        {
            // arrange
            int testId = 10;
            var expectedEntityState = EntityState.Unchanged;

            _catalogItemRepository.Setup(x => x.RemoveAsync(It.Is<int>(i => i == testId))).ReturnsAsync(expectedEntityState);

            // act
            bool actualResult = await _catalogItemService.RemoveAsync(testId);

            // assert
            actualResult.Should().BeFalse();
        }
    }
}
