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
    [Route(ComponentDefaults.BasketDefaultRoute)]
    public class BasketBffController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketBffController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SetItemsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetItemsToBasketAsync(SetItemsRequest request)
        {
            string? basketId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
            bool isSet = await _basketService.SetItemsAsync(basketId, request.Items.ToList());

            return Ok(new SetItemsResponse { IsSet = isSet });
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetItemsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemsFromBasketAsync()
        {
            string? basketId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
            GetItemsResponse? response = await _basketService.GetItemsAsync(basketId);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FlushAllResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FlushAllAsync()
        {
            string? basketId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
            bool isFlushed = await _basketService.FlushAllAsync(basketId);

            return Ok(new FlushAllResponse { IsFlushed = isFlushed });
        }
    }
}
