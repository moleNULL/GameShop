using WebAPI.Models.Dtos;
using WebAPI.Models.Enums;
using WebAPI.Models.Responses;

namespace WebAPI.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<CatalogItemDto?> GetItemByIdAsync(int id);
        Task<IEnumerable<CatalogItemDto>> GetItemByCompanyAsync(string company);
        Task<IEnumerable<CatalogItemDto>> GetItemByGenreAsync(string genre);
        Task<IEnumerable<CatalogCompanyDto>> GetCompaniesAsync();
        Task<IEnumerable<CatalogGenreDto>> GetGenresAsync();
        Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(
            int pageIndex, int pageSize, Dictionary<CatalogTypeFilter, int?> filters);
        Task<IEnumerable<CatalogItemDto>> GetAllCatalogItemsAsync();
    }
}
