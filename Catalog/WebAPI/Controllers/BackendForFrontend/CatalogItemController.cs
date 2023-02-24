﻿using System.Net;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Requests;
using WebAPI.Models.Responses;
using WebAPI.Services.Implementations;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers.BackendForFrontend
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
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

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadAsync(ItemByIdRequest request)
        {
            var result = await _catalogItemService.GetAsync(request.Id);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync(UpdateItemRequest request)
        {
            var result = await _catalogItemService.UpdateAsync(request.Id, request.Name, request.Price, request.Year, request.PictureFileName, request.AvailableStock, request.CatalogCompanyId, request.CatalogGenreId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RemoveItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(RemoveItemRequest request)
        {
            var result = await _catalogItemService.RemoveAsync(request.Id);

            return Ok(result);
        }
    }
}
