using System.Net;
using Catalog.WebAPI.Models.Responses;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Dtos;
using WebAPI.Models.Enums;
using WebAPI.Models.Requests;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;

namespace Catalog.WebAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.CatalogDefaultRoute)] // Route("api/v1/[controller]/[action]")
    public class CatalogBffController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogBffController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCatalogItemsAsync(PaginatedItemsRequest<CatalogTypeFilter> request)
        {
            var result = await _catalogService.GetCatalogItemsAsync(
                request.PageIndex, request.PageSize, request.Filters);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCatalogItemsAsync()
        {
            var result = await _catalogService.GetAllCatalogItemsAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemByIdAsync(ItemByIdRequest request)
        {
            var result = await _catalogService.GetItemByIdAsync(request.Id);

            return Ok(new GetItemResponse { Item = result });
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
        [ProducesResponseType(typeof(IEnumerable<CatalogCompanyDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCompaniesAsync()
        {
            var result = await _catalogService.GetCompaniesAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CatalogGenreDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGenresAsync()
        {
            var result = await _catalogService.GetGenresAsync();

            return Ok(result);
        }
    }
}
