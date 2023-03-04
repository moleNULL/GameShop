using WebAPI.Data;
using WebAPI.Models.Dtos;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Implementations;
using WebAPI.Services.Interfaces;

namespace Catalog.UnitTests.Services
{
    public class CatalogServiceTest
    {
        private readonly ICatalogService _catalogService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerBaseData;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

        private CatalogItemEntity _testItemEntity = new CatalogItemEntity()
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

        private CatalogItemDto _testItemDto = new CatalogItemDto()
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

        public CatalogServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _mapper = new Mock<IMapper>();
            _loggerBaseData = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _logger = new Mock<ILogger<CatalogService>>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbContextTransaction.Object);

            _catalogService = new CatalogService(
                _dbContextWrapper.Object,
                _loggerBaseData.Object,
                _logger.Object,
                _catalogItemRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Success()
        {
            // arrange
            int testPageIndex = 0;
            int testPageSize = 6;
            int totalCount = 1;
            var paginatedItemsExpectedResult = new PaginatedItems<CatalogItemEntity>()
            {
                TotalCount = totalCount,
                ItemList = new List<CatalogItemEntity>() { _testItemEntity }
            };

            _catalogItemRepository.Setup(x => x.GetItemsByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize),
                It.IsAny<int?>(),
                It.IsAny<int?>())).ReturnsAsync(paginatedItemsExpectedResult);

            _mapper.Setup(x => x.Map<CatalogItemDto>(
                It.Is<CatalogItemEntity>(i => i.Equals(_testItemEntity)))).Returns(_testItemDto);

            // act
            var actualResult = await _catalogService.GetCatalogItemsAsync(
                testPageIndex, testPageSize, null!);

            // assert
            actualResult.Should().NotBeNull();
            actualResult?.Count.Should().Be(totalCount);
            actualResult?.PageSize.Should().Be(testPageSize);
            actualResult?.PageIndex.Should().Be(testPageIndex);
            actualResult?.ItemList.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Failed()
        {
            // arrange
            int testPageIndex = 100;
            int testPageSize = 600;
            PaginatedItems<CatalogItemEntity> testPaginatedItems = null!;

            _catalogItemRepository.Setup(x => x.GetItemsByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize),
                It.IsAny<int?>(),
                It.IsAny<int?>())).ReturnsAsync(testPaginatedItems);

            // act
            var actualResult = await _catalogService.GetCatalogItemsAsync(testPageIndex, testPageSize, null!);

            // assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogItemsAsync_ThrowsException()
        {
            // arrange
            int testPageIndex = -5;
            int testPageSize = -3;

            // act
            var act = async () => await _catalogService.GetCatalogItemsAsync(
                testPageIndex, testPageSize, null!);

            // assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("pageIndex or pageSize must not be negative");
        }

        [Fact]
        public async Task GetAllCatalogItemsAsync_Success()
        {
            // arrange
            var expectedEntitiesResult = new List<CatalogItemEntity>() { _testItemEntity };
            var expectedDtosResult = new List<CatalogItemDto>() { _testItemDto };

            _catalogItemRepository.Setup(x => x.GetAllCatalogItemsAsync()).ReturnsAsync(expectedEntitiesResult);
            _mapper.Setup(x => x.Map<IEnumerable<CatalogItemDto>>(
                It.Is<IEnumerable<CatalogItemEntity>>(i => i.Equals(expectedEntitiesResult))))
            .Returns(expectedDtosResult);

            // act
            var actualResult = await _catalogService.GetAllCatalogItemsAsync();

            // assert
            actualResult.Should().BeEquivalentTo(expectedDtosResult);
        }
    }
}