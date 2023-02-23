using WebAPI.Models.Dtos;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;
using Infrastructure.Services;
using CatalogWebAPI.Data;
using Infrastructure.Services.Interfaces;
using WebAPI.Repositories.Interfaces;
using AutoMapper;
using CatalogWebAPI.Data.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Services.Implementations
{
    public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> loggerBaseData,
            ILogger<CatalogService> logger,
            ICatalogItemRepository catalogItemRepository,
            IMapper mapper)
            : base(dbContextWrapper, loggerBaseData)
        {
            _catalogItemRepository = catalogItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CatalogItemDto> GetItemByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetByIdAsync(id);
                object resultDto = _mapper.Map(resultEntity, typeof(CatalogItemEntity), typeof(CatalogItemDto));

                return (CatalogItemDto)resultDto;
            });
        }

        public async Task<List<CatalogItemDto>> GetItemByCompanyAsync(string company)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetByCompanyAsync(company);
                object resultDto = _mapper.Map(resultEntity, typeof(List<CatalogItemEntity>), typeof(List<CatalogItemDto>));

                return (List<CatalogItemDto>)resultDto;
            });
        }

        public async Task<List<CatalogItemDto>> GetItemByGenreAsync(string genre)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetByGenreAsync(genre);
                object resultDto = _mapper.Map(resultEntity, typeof(List<CatalogItemEntity>), typeof(List<CatalogItemDto>));

                return (List<CatalogItemDto>)resultDto;
            });
        }

        public async Task<List<CatalogCompanyDto>> GetCompaniesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetCompaniesAsync();
                object resultDto = _mapper.Map(resultEntity, typeof(List<CatalogCompanyEntity>), typeof(List<CatalogCompanyDto>));

                return (List<CatalogCompanyDto>)resultDto;
            });
        }

        public async Task<List<CatalogGenreDto>> GetGenresAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetGenresAsync();
                object resultDto = _mapper.Map(resultEntity, typeof(List<CatalogGenreEntity>), typeof(List<CatalogGenreDto>));

                return (List<CatalogGenreDto>)resultDto;
            });
        }

        public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageIndex, int pageSize)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (pageIndex < 0 || pageSize < 0)
                {
                    _logger.LogWarning($"pageIndex or pageSize must be negative");
                    throw new Exception("pageIndex or pageSize must be negative");
                }

                var result = await _catalogItemRepository.GetItemsByPageAsync(pageIndex, pageSize);
                return new PaginatedItemsResponse<CatalogItemDto>()
                {
                    Count = result.TotalCount,
                    ItemList = result.ItemList?.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
        }
    }
}
