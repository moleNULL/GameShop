using Basket.WebAPI.Models;

namespace Basket.WebAPI.Services.Interfaces
{
    public interface IBasketService
    {
        Task SetItemAsync(
            string? userId, int itemId, string itemName, decimal itemPrice, string itemPictureUrl, int itemQuantity);

        Task<GetItemResponse> GetItemAsync(string? userId);
    }
}
