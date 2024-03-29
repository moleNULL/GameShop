﻿using System.Net;
using Catalog.WebAPI.Models.Responses;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Requests;
using WebAPI.Models.Responses;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("catalog.catalogitem")]
    [Route(ComponentDefaults.CatalogDefaultRoute)]
    public class CatalogItemController : ControllerBase
    {
        private readonly ICatalogItemService _catalogItemService;

        public CatalogItemController(ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAsync(CreateItemRequest request)
        {
            var result = await _catalogItemService.AddAsync(
                request.Name, request.Price, request.Year, request.PictureFileName, request.AvailableStock, request.CatalogCompanyId, request.CatalogGenreId);

            return Ok(new AddItemResponse<int?> { ItemId = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadAsync(ItemByIdRequest request)
        {
            var result = await _catalogItemService.GetAsync(request.Id);

            return Ok(new GetItemResponse { Item = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync(UpdateItemRequest request)
        {
            var result = await _catalogItemService.UpdateAsync(request.Id, request.Name, request.Price, request.Year, request.PictureFileName, request.AvailableStock, request.CatalogCompanyId, request.CatalogGenreId);

            return Ok(new UpdateItemResponse<bool> { IsUpdated = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(RemoveItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(RemoveItemRequest request)
        {
            var result = await _catalogItemService.RemoveAsync(request.Id);

            return Ok(new RemoveItemResponse<bool> { IsRemoved = result });
        }
    }
}
