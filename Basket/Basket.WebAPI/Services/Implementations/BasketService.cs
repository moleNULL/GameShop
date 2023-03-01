using Basket.WebAPI.Models;
using Basket.WebAPI.Services.Interfaces;
using Infrastructure.Exceptions;

namespace Basket.WebAPI.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly ICacheService _cacheService;

        public BasketService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<GetItemResponse> GetItemAsync(string? userId)
        {
            if (userId is null)
            {
                throw new BusinessException("userId is null");
            }

            string result = await _cacheService.GetAsync<string>(userId);

            // incorrect data
            return new GetItemResponse() { ItemName = result };
        }

        public async Task SetItemAsync(string? userId, int itemId, string itemName, decimal itemPrice, string itemPictureUrl, int itemQuantity)
        {
            if (userId is null)
            {
                throw new BusinessException("userId is null");
            }

            await _cacheService.SetAsync(userId, itemName);
        }
    }
}
