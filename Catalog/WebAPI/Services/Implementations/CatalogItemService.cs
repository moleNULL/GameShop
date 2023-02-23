using CatalogWebAPI.Data;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Implementations;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services.Implementations
{
    public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly ILogger<CatalogItemService> _logger;

        public CatalogItemService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> loggerBaseData,
            ILogger<CatalogItemService> logger,
            ICatalogItemRepository catalogItemRepository)
            : base(dbContextWrapper, loggerBaseData)
        {
            _catalogItemRepository = catalogItemRepository;
            _logger = logger;
        }

        public Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId)
        {
            return ExecuteSafeAsync(() =>
            {
                return _catalogItemRepository.AddAsync(
                    name, price, year, pictureFileName, availableStock, companyId, genreId);
            });
        }

        public Task<bool> RemoveAsync(int id)
        {
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

        public Task<bool> UpdateAsync(int id, string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId)
        {
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
    }
}
