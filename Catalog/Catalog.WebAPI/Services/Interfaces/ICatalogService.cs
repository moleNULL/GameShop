using WebAPI.Models.Dtos;
using WebAPI.Models.Enums;
using WebAPI.Models.Responses;

namespace WebAPI.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<CatalogItemDto> GetItemByIdAsync(int id);
        Task<List<CatalogItemDto>> GetItemByCompanyAsync(string company);
        Task<List<CatalogItemDto>> GetItemByGenreAsync(string genre);
        Task<List<CatalogCompanyDto>> GetCompaniesAsync();
        Task<List<CatalogGenreDto>> GetGenresAsync();
        Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(
            int pageIndex, int pageSize, Dictionary<CatalogTypeFilter, int?> filters);
    }
}
