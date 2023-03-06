using WebAPI.Models.Dtos;

namespace Catalog.WebAPI.Models.Responses
{
    public class GetItemResponse
    {
        public CatalogItemDto? Item { get; init; }
    }
}
