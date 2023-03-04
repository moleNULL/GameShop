using AutoMapper;
using CatalogWebAPI.Data;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Dtos;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services.Implementations
{
    public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly ILogger<CatalogItemService> _logger;
        private readonly IMapper _mapper;

        public CatalogItemService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> loggerBaseData,
            ILogger<CatalogItemService> logger,
            ICatalogItemRepository catalogItemRepository,
            IMapper mapper)
            : base(dbContextWrapper, loggerBaseData)
        {
            _catalogItemRepository = catalogItemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId)
        {
            return ExecuteSafeAsync(() =>
            {
                return _catalogItemRepository.AddAsync(
                    name, price, year, pictureFileName, availableStock, companyId, genreId);
            });
        }

        public Task<CatalogItemDto?> GetAsync(int id)
        {
            if (id < 1)
            {
                throw new BusinessException($"Id must not be 0 or negative. Provided id: {id}");
            }

            return ExecuteSafeAsync(async () =>
            {
                var resultEntity = await _catalogItemRepository.GetByIdAsync(id);
                var resultDto = _mapper.Map<CatalogItemDto?>(resultEntity);

                return resultDto;
            });
        }

        public Task<bool> UpdateAsync(int id, string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId)
        {
            if (id < 1)
            {
                throw new BusinessException($"Id must not be 0 or negative. Provided id: {id}");
            }

            return ExecuteSafeAsync(async () =>
            {
                var result = await _catalogItemRepository.UpdateAsync(id, name, price, year, pictureFileName, availableStock, companyId, genreId);

                if (result == EntityState.Modified)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public Task<bool> RemoveAsync(int id)
        {
            if (id < 1)
            {
                throw new BusinessException($"Id must not be 0 or negative. Provided id: {id}");
            }

            return ExecuteSafeAsync(async () =>
            {
                var result = await _catalogItemRepository.RemoveAsync(id);

                if (result == EntityState.Deleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
    }
}
