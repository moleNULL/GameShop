using CatalogWebAPI.Data.Entities;
using WebAPI.Data;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICatalogItemRepository
    {
        Task<PaginatedItems<CatalogItemEntity>> GetItemsByPageAsync(int pageIndex, int pageSize);
        Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availablestock, int companyId, int genreId);
    }
}
