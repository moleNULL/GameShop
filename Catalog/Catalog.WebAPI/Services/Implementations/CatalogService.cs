﻿using WebAPI.Models.Dtos;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;
using Infrastructure.Services;
using CatalogWebAPI.Data;
using Infrastructure.Services.Interfaces;
using WebAPI.Repositories.Interfaces;
using AutoMapper;
using WebAPI.Models.Enums;

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

        public async Task<CatalogItemDto?> GetItemByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetByIdAsync(id);
                var resultDto = _mapper.Map<CatalogItemDto?>(resultEntity);

                return resultDto;
            });
        }

        public async Task<IEnumerable<CatalogItemDto>> GetItemByCompanyAsync(string company)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntities = await _catalogItemRepository.GetByCompanyAsync(company);
                var resultDtos = _mapper.Map<List<CatalogItemDto>>(resultEntities);

                return resultDtos;
            });
        }

        public async Task<IEnumerable<CatalogItemDto>> GetItemByGenreAsync(string genre)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntities = await _catalogItemRepository.GetByGenreAsync(genre);
                var resultDtos = _mapper.Map<List<CatalogItemDto>>(resultEntities);

                return resultDtos;
            });
        }

        public async Task<IEnumerable<CatalogCompanyDto>> GetCompaniesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntities = await _catalogItemRepository.GetCompaniesAsync();
                var resultDto = _mapper.Map<List<CatalogCompanyDto>>(resultEntities);

                return resultDto;
            });
        }

        public async Task<IEnumerable<CatalogGenreDto>> GetGenresAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetGenresAsync();
                var resultDto = _mapper.Map<List<CatalogGenreDto>>(resultEntity);

                return resultDto;
            });
        }

        public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(
            int pageIndex, int pageSize, Dictionary<CatalogTypeFilter, int?> filters)
        {
            return await ExecuteSafeAsync(async () =>
            {
                int? companyId = null;
                int? genreId = null;

                if (filters is not null)
                {
                    if (filters.TryGetValue(CatalogTypeFilter.CompanyId, out int? company))
                    {
                        // 0 - All Companies, so companyId must be null to select all items
                        if (company != 0)
                        {
                            companyId = company;
                        }
                    }

                    if (filters.TryGetValue(CatalogTypeFilter.GenreId, out int? genre))
                    {
                        // 0 - All Genres, so genreId must be null to select all items
                        if (genre != 0)
                        {
                            genreId = genre;
                        }
                    }
                }

                var result = await _catalogItemRepository.GetItemsByPageAsync(
                    pageIndex, pageSize, companyId, genreId);

                return new PaginatedItemsResponse<CatalogItemDto>()
                {
                    Count = result.TotalCount,
                    ItemList = result.ItemList?.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
        }

        public async Task<IEnumerable<CatalogItemDto>> GetAllCatalogItemsAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var resultEntities = await _catalogItemRepository.GetAllCatalogItemsAsync();
                var resultDto = _mapper.Map<IEnumerable<CatalogItemDto>>(resultEntities);

                return resultDto;
            });
        }
    }
}
