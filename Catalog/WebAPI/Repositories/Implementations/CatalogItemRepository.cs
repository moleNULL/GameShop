using System.Net.WebSockets;
using CatalogWebAPI.Data;
using CatalogWebAPI.Data.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using WebAPI.Data;
using WebAPI.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public async Task<CatalogItemEntity> GetByIdAsync(int id)
        {
            var item = await _dbContext.CatalogItems
                .Include(ci => ci.CatalogCompany)
                .Include(ci => ci.CatalogGenre)
                .FirstOrDefaultAsync(ci => ci.Id == id);

            return item ?? new CatalogItemEntity();
        }

        public async Task<List<CatalogItemEntity>> GetByCompanyAsync(string company)
        {
            var items = await _dbContext.CatalogItems
                .Include(ci => ci.CatalogCompany)
                .Include(ci => ci.CatalogGenre)
                .Where(ci => ci.CatalogCompany.Company == company)
                .ToListAsync();

            return items ?? new List<CatalogItemEntity>();
        }

        public async Task<List<CatalogItemEntity>> GetByGenreAsync(string genre)
        {
            var items = await _dbContext.CatalogItems
                .Include(ci => ci.CatalogCompany)
                .Include(ci => ci.CatalogGenre)
                .Where(ci => ci.CatalogGenre.Genre == genre)
                .ToListAsync();

            return items ?? new List<CatalogItemEntity>();
        }

        public async Task<List<CatalogCompanyEntity>> GetCompaniesAsync()
        {
            var companies = await _dbContext.CatalogCompanies.ToListAsync();
            return companies ?? new List<CatalogCompanyEntity>();
        }

        public async Task<List<CatalogGenreEntity>> GetGenresAsync()
        {
            var genres = await _dbContext.CatalogGenres.ToListAsync();
            return genres ?? new List<CatalogGenreEntity>();
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

        public async Task<EntityState> RemoveAsync(int id)
        {
            var result = _dbContext.CatalogItems.Remove(new CatalogItemEntity() { Id = id });
            EntityState state = result.State;

            await _dbContext.SaveChangesAsync();

            return state;
        }

        public async Task<EntityState> UpdateAsync(int id, string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId)
        {
            CatalogItemEntity entity = _dbContext.CatalogItems.FirstOrDefault(ci => ci.Id == id) !;

            entity.Name = name;
            entity.Price = price;
            entity.Year = year;
            entity.PictureFileName = pictureFileName;
            entity.AvailableStock = availableStock;
            entity.CatalogCompanyId = companyId;
            entity.CatalogGenreId = genreId;

            var result = _dbContext.CatalogItems.Update(entity);
            EntityState state = result.State;

            await _dbContext.SaveChangesAsync();

            return state;
        }
    }
}
