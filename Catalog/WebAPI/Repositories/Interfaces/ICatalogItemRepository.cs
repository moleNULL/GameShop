using CatalogWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICatalogItemRepository
    {
        Task<CatalogItemEntity> GetByIdAsync(int id);
        Task<List<CatalogItemEntity>> GetByCompanyAsync(string company);
        Task<List<CatalogItemEntity>> GetByGenreAsync(string genre);
        Task<List<CatalogCompanyEntity>> GetCompaniesAsync();
        Task<List<CatalogGenreEntity>> GetGenresAsync();
        Task<PaginatedItems<CatalogItemEntity>> GetItemsByPageAsync(int pageIndex, int pageSize);

        Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availablestock, int companyId, int genreId);
        Task<EntityState> RemoveAsync(int id);
        Task<EntityState> UpdateAsync(int id, string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId);
    }
}
