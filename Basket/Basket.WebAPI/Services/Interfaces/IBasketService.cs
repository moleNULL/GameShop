using Basket.WebAPI.Models;

namespace Basket.WebAPI.Services.Interfaces
{
    public interface IBasketService
    {
        Task<bool> SetItemsAsync(string? userId, List<ItemDto> items);

        Task<GetItemsResponse> GetItemsAsync(string? userId);

        Task<bool> FlushAllAsync(string? userId);
    }
}
