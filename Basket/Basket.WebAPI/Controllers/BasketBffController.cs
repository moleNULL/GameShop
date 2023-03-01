using System.Net;
using Basket.WebAPI.Models;
using Basket.WebAPI.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.WebAPI.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketBffController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketBffController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetItemAsync(SetItemRequest request)
        {
            string? basketId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

            await _basketService.SetItemAsync(basketId, request.ItemId, request.ItemName, request.ItemPrice, request.ItemPictureUrl, request.ItemQuantity);

            // fill it with data
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemAsync()
        {
            string? basketId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
            GetItemResponse? response = await _basketService.GetItemAsync(basketId);

            return Ok(response);
        }
    }
}
