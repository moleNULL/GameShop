using WebAPI.Models.Dtos;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;
using Infrastructure.Services;
using CatalogWebAPI.Data;
using Infrastructure.Services.Interfaces;
using WebAPI.Repositories.Interfaces;
using AutoMapper;

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

        public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageIndex, int pageSize)
        {
            return await ExecuteSafeAsync(async () =>
            {
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
