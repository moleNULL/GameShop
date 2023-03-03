using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MVC.Configurations;
using MVC.Models;
using MVC.Models.BasketModels;
using MVC.Models.CatalogDtos;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        private readonly IOptions<MvcConfig> _settings;
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(
            IHttpClientService httpClientService, ILogger<CatalogService> logger, IOptions<MvcConfig> settings)
        {
            _httpClientService = httpClientService;
            _logger = logger;
            _settings = settings;
        }

        public async Task<Catalog> GetCatalogItemsAsync(int page, int itemsPerPage, int? companyFilter, int? genreFilter)
        {
            var filters = new Dictionary<CatalogTypeFilter, int?>();

            if (companyFilter.HasValue)
            {
                filters.Add(CatalogTypeFilter.CompanyId, companyFilter.Value);
            }

            if (genreFilter.HasValue)
            {
                filters.Add(CatalogTypeFilter.GenreId, genreFilter.Value);
            }

            var result = await _httpClientService.SendAsync<Catalog, PaginatedItemRequest<CatalogTypeFilter>>(
                $"{_settings.Value.CatalogUrl}/getcatalogitems",
                HttpMethod.Post,
                new PaginatedItemRequest<CatalogTypeFilter>()
                {
                    PageIndex = page,
                    PageSize = itemsPerPage,
                    Filters = filters
                });

            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetCompaniesAsync()
        {
            var companiesResult = await _httpClientService.SendAsync<List<CatalogCompany>, object>(
                $"{_settings.Value.CatalogUrl}/getcompanies",
                HttpMethod.Post,
                null);

            var allCompaniesItem = new SelectListItem() { Value = "0", Text = "All Companies" };

            var companiesSelectList = new List<SelectListItem>() { allCompaniesItem };
            companiesSelectList.AddRange(companiesResult.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Company
            }));

            return companiesSelectList;
        }

        public async Task<IEnumerable<SelectListItem>> GetGenresAsync()
        {
            var genresResult = await _httpClientService.SendAsync<List<CatalogGenre>, object>(
                $"{_settings.Value.CatalogUrl}/getgenres",
                HttpMethod.Post,
                null);

            var allGenresItem = new SelectListItem() { Value = "0", Text = "All Genres" };

            var genresSelectList = new List<SelectListItem>() { allGenresItem };

            genresSelectList.AddRange(genresResult.Select(g => new SelectListItem()
            {
                Value = g.Id.ToString(),
                Text = g.Genre
            }));

            return genresSelectList;
        }

        public async Task<IEnumerable<CatalogItemDto>> GetAllCatalogItemsAsync()
        {
            var allItemsResult = await _httpClientService.SendAsync<IEnumerable<CatalogItemDto>, object>(
                $"{_settings.Value.CatalogUrl}/getallcatalogitems",
                HttpMethod.Post,
                null);

            return allItemsResult;
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
            string url = $"{_settings.Value.BasketUrl}/flushall";
            var result = await _httpClientService.SendAsync<EmptyBasketResponse, object>(
                url, HttpMethod.Post, null);

            if (result is null)
            {
                throw new BusinessException(
                    $"Error! Unable to get data about emptying from Basket.WebAPI. Request URL: {url}");
            }

            return result.IsFlushed;
        }
    }
}
