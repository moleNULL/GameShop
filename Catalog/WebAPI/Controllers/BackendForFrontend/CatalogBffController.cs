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
    }
}
