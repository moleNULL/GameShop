using CatalogWebAPI.Data;
using CatalogWebAPI.Data.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories.Implementations
{
    public class CatalogItemRepository : ICatalogItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        // info: WebAPI.Repositories.Implementations.CatalogItemRepository[0]
        // Log message here (unique forr CatalogItemRepository)
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogItemRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availablestock, int companyId, int genreId)
        {
            var item = new CatalogItemEntity()
            {
                Name = name,
                Price = price,
                Year = year,
                PictureFileName = pictureFileName,
                AvailableStock = availablestock,
                CatalogCompanyId = companyId,
                CatalogGenreId = genreId
            };

            var itemInfo = await _dbContext.CatalogItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return itemInfo.Entity.Id;
        }

        public async Task<PaginatedItems<CatalogItemEntity>> GetItemsByPageAsync(int pageIndex, int pageSize)
        {
            int totalCount = await _dbContext.CatalogItems.CountAsync();

            var itemList = await _dbContext.CatalogItems
                .Include(i => i.CatalogCompany)
                .Include(i => i.CatalogGenre)
                .OrderBy(ci => ci.Id)
                .Skip(pageIndex * pageSize) // 0 * 6 = skip 0, then 1 * 6 - skip first 6 items and so on
                .Take(pageSize) // take 6 items
                .ToListAsync();

            return new PaginatedItems<CatalogItemEntity>() { TotalCount = totalCount, ItemList = itemList };
        }
    }
}
