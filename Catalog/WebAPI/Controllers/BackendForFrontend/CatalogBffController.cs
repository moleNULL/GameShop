using System.Net;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Dtos;
using WebAPI.Models.Requests;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers.BackendForFrontend
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)] // Route("api/v1/[controller]/[action]")
    public class CatalogBffController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly ILogger<CatalogBffController> _logger;

        public CatalogBffController(
            ILogger<CatalogBffController> logger,
            ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCatalogItemsAsync(PaginatedItemsRequest request)
        {
            var result = await _catalogService.GetCatalogItemsAsync(request.PageIndex, request.PageSize);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemByIdAsync(ItemByIdRequest request)
        {
            var result = await _catalogService.GetItemByIdAsync(request.Id);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemByCompanyAsync(ItemByCompanyRequest request)
        {
            var result = await _catalogService.GetItemByCompanyAsync(request.Company);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemsByGenreAsync(ItemByGenreRequest request)
        {
            var result = await _catalogService.GetItemByGenreAsync(request.Genre);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CompanyResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCompaniesAsync()
        {
            var result = await _catalogService.GetCompaniesAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GenreResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGenresAsync()
        {
            var result = await _catalogService.GetGenresAsync();

            return Ok(result);
        }
    }
}
