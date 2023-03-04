using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.BasketModels;
using Newtonsoft.Json;
using MVC.Models.CatalogDtos;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            var catalogItems = await _catalogService.GetAllCatalogItemsAsync() ?? new List<CatalogItemDto>();

            return View(catalogItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(List<string> items)
        {
            var basketItems = new List<BasketItemDto>();

            foreach (var itemJson in items)
            {
                var item = JsonConvert.DeserializeObject<BasketItemDto>(itemJson);

                if (item is not null)
                {
                    basketItems.Add(item);
                }
            }

            if (basketItems.Count == 0)
            {
                ViewData["isAdded"] = false;
            }
            else
            {
                bool isAdded = await _basketService.AddItemsToBasketAsync(basketItems);
                ViewData["isAdded"] = isAdded;
            }

            var catalogItems = await _catalogService.GetAllCatalogItemsAsync() ?? new List<CatalogItemDto>();

            return View(catalogItems);
        }

        [Authorize]
        public async Task<IActionResult> ShowAsync()
        {
            var basketItems = await _basketService.GetItemsFromBasketAsync() ?? new List<BasketItemDto>();

            return View(basketItems);
        }

        [Authorize]
        public async Task<IActionResult> EmptyAsync()
        {
            await _basketService.EmptyBasketAsync();

            return RedirectToAction("Show");
        }
    }
}
