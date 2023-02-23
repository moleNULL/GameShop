using WebAPI.Models.Dtos;
using WebAPI.Models.Responses;

namespace WebAPI.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageIndex, int pageSize);
    }
}
