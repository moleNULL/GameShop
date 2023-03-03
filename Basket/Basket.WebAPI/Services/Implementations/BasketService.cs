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

        public async Task<GetItemsResponse> GetItemsAsync(string? userId)
        {
            if (userId is null)
            {
                throw new BusinessException("Cannot get items because userId is null");
            }

            var result = await _cacheService.GetAsync<List<ItemDto>>(userId);
            return new GetItemsResponse() { Items = result };
        }

        public async Task<bool> SetItemsAsync(string? userId, List<ItemDto> items)
        {
            if (userId is null)
            {
                throw new BusinessException("Cannot set items because userId is null");
            }

            bool isSet = await _cacheService.SetAsync(userId, items);
            return isSet;
        }

        public async Task<bool> FlushAllAsync(string? userId)
        {
            if (userId is null)
            {
                throw new BusinessException("Cannot delete data because userId is null");
            }

            bool isFlushed = await _cacheService.FlushAsync(userId);
            return isFlushed;
        }
    }
}
