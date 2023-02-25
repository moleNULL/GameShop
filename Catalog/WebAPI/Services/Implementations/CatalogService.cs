using WebAPI.Models.Dtos;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;
using Infrastructure.Services;
using CatalogWebAPI.Data;
using Infrastructure.Services.Interfaces;
using WebAPI.Repositories.Interfaces;
using AutoMapper;
using CatalogWebAPI.Data.Entities;
using Infrastructure.Exceptions;

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
                if (id < 1)
                {
                    throw new BusinessException($"Id must not be negative. Provided id: {id}");
                }

                var resultEntity = await _catalogItemRepository.GetByIdAsync(id);
                var resultDto = _mapper.Map<CatalogItemDto>(resultEntity);

                return resultDto;
            });
        }

        public async Task<List<CatalogItemDto>> GetItemByCompanyAsync(string company)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(company))
                {
                    throw new BusinessException($"Company must not be null, empty or only whitespaces");
                }

                var resultEntities = await _catalogItemRepository.GetByCompanyAsync(company);
                var resultDtos = _mapper.Map<List<CatalogItemDto>>(resultEntities);

                return resultDtos;
            });
        }

        public async Task<List<CatalogItemDto>> GetItemByGenreAsync(string genre)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(genre))
                {
                    throw new BusinessException($"Genre must not be null, empty or only whitespaces");
                }

                var resultEntities = await _catalogItemRepository.GetByGenreAsync(genre);
                var resultDtos = _mapper.Map<List<CatalogItemDto>>(resultEntities);

                return resultDtos;
            });
        }

        public async Task<List<CatalogCompanyDto>> GetCompaniesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntities = await _catalogItemRepository.GetCompaniesAsync();
                var resultDto = _mapper.Map<List<CatalogCompanyDto>>(resultEntities);

                return resultDto;
            });
        }

        public async Task<List<CatalogGenreDto>> GetGenresAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetGenresAsync();
                var resultDto = _mapper.Map<List<CatalogGenreDto>>(resultEntity);

                return resultDto;
            });
        }

        public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageIndex, int pageSize)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (pageIndex < 0 || pageSize < 0)
                {
                    throw new BusinessException("pageIndex or pageSize must not be negative");
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
