using CatalogWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICatalogItemRepository
    {
        Task<CatalogItemEntity> GetByIdAsync(int id);
        Task<IEnumerable<CatalogItemEntity>> GetByCompanyAsync(string company);
        Task<IEnumerable<CatalogItemEntity>> GetByGenreAsync(string genre);
        Task<IEnumerable<CatalogCompanyEntity>> GetCompaniesAsync();
        Task<IEnumerable<CatalogGenreEntity>> GetGenresAsync();
        Task<PaginatedItems<CatalogItemEntity>> GetItemsByPageAsync(
            int pageIndex, int pageSize, int? companyId, int? genreId);
        Task<IEnumerable<CatalogItemEntity>> GetAllCatalogItemsAsync();

        Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availablestock, int companyId, int genreId);
        Task<EntityState> RemoveAsync(int id);
        Task<EntityState> UpdateAsync(int id, string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId);
    }
}
