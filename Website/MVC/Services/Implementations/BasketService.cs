using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using MVC.Configurations;
using MVC.Models.BasketModels;
using MVC.Services.Interfaces;

namespace MVC.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IOptions<MvcConfig> _settings;
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<BasketService> _logger;

        public BasketService(
            IHttpClientService httpClientService, ILogger<BasketService> logger, IOptions<MvcConfig> settings)
        {
            _httpClientService = httpClientService;
            _logger = logger;
            _settings = settings;
        }

        public async Task<bool> AddItemsToBasketAsync(IEnumerable<BasketItemDto> items)
        {
            string url = $"{_settings.Value.BasketUrl}/setitemstobasket";
            var result = await _httpClientService.SendAsync<AddItemsToBasketResponse, AddItemsToBasketRequest>(
                url, HttpMethod.Post, new AddItemsToBasketRequest { Items = items });

            if (result is null)
            {
                throw new BusinessException(
                    $"Error! Unable to set data to Basket.WebAPI. Request URL: {url}");
            }

            return result.IsSet;
        }

        public async Task<IEnumerable<BasketItemDto>> GetItemsFromBasketAsync()
        {
            string url = $"{_settings.Value.BasketUrl}/getitemsfrombasket";
            var result = await _httpClientService.SendAsync<GetItemsFromBasketResponse, object>(
                url, HttpMethod.Post, null);

            if (result is null)
            {
                throw new BusinessException(
                    $"Error! Unable to get data from Basket.WebAPI. Request URL: {url}");
            }

            return result.Items;
        }

        public async Task<bool> EmptyBasketAsync()
        {
            string url = $"{_settings.Value.BasketUrl}/deleteitems";
            var result = await _httpClientService.SendAsync<EmptyBasketResponse, object>(
                url, HttpMethod.Post, null);

            if (result is null)
            {
                throw new BusinessException(
                    $"Error! Unable to get data about emptying from Basket.WebAPI. Request URL: {url}");
            }

            var items = await GetItemsFromBasketAsync();
            if (items is not null && result.IsDeleted == false)
            {
                throw new BusinessException($"Error! Failed to delete items from Basket.WebAPI");
            }

            return result.IsDeleted;
        }
    }
}
