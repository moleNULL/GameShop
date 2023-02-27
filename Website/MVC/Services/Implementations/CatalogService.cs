using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MVC.Configurations;
using MVC.Models;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using System.Net.Http;

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

        public async Task<Catalog> GetCatalogItems(int page, int itemsPerPage, int? companyFilter, int? genreFilter)
        {
            var filters = new Dictionary<CatalogTypeFilter, int?>();

            if (companyFilter.HasValue)
            {
                filters.Add(CatalogTypeFilter.Company, companyFilter.Value);
            }

            if (genreFilter.HasValue)
            {
                filters.Add(CatalogTypeFilter.Genre, genreFilter.Value);
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

        public async Task<IEnumerable<SelectListItem>> GetCompanies()
        {
            var companiesResult = await _httpClientService.SendAsync<List<CatalogCompany>, object>(
                $"{_settings.Value.CatalogUrl}/getcompanies",
                HttpMethod.Post,
                null);

            var companiesSelectList = companiesResult.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Company
            });

            return companiesSelectList;
        }

        public async Task<IEnumerable<SelectListItem>> GetGenres()
        {
            var genresResult = await _httpClientService.SendAsync<List<CatalogGenre>, object>(
                $"{_settings.Value.CatalogUrl}/getgenres",
                HttpMethod.Post,
                null);

            var genresSelectList = genresResult.Select(g => new SelectListItem()
            {
                Value = g.Id.ToString(),
                Text = g.Genre
            });

            return genresSelectList;
        }
    }
}
