using MVC.Models.BasketModels;

namespace MVC.Services.Interfaces
{
    public interface IBasketService
    {
        Task<bool> AddItemsToBasketAsync(IEnumerable<BasketItemDto> items);
        Task<IEnumerable<BasketItemDto>> GetItemsFromBasketAsync();
        Task<bool> EmptyBasketAsync();
    }
}
