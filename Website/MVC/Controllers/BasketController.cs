using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.BasketModels;
using Newtonsoft.Json;
using MVC.Models.CatalogDtos;
using MVC.Services.Interfaces;
using Infrastructure.Exceptions;

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
            IEnumerable<CatalogItemDto>? catalogItems;

            try
            {
                catalogItems = await _catalogService.GetAllCatalogItemsAsync();
            }
            catch (BusinessException)
            {
                return View("Error");
            }

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

            IEnumerable<CatalogItemDto>? catalogItems;

            try
            {
                if (basketItems.Count == 0)
                {
                    ViewData["isAdded"] = false;
                }
                else
                {
                    bool isAdded = await _basketService.AddItemsToBasketAsync(basketItems);

                    ViewData["isAdded"] = isAdded;
                }

                catalogItems = await _catalogService.GetAllCatalogItemsAsync();
            }
            catch (BusinessException)
            {
                return View("Error");
            }

            return View(catalogItems);
        }

        public async Task<IActionResult> ShowAsync()
        {
            IEnumerable<BasketItemDto> basketItems;

            try
            {
                basketItems = await _basketService.GetItemsFromBasketAsync();
            }
            catch (BusinessException)
            {
                return View("Error");
            }

            return View(basketItems);
        }

        public async Task<IActionResult> EmptyAsync()
        {
            try
            {
                await _basketService.EmptyBasketAsync();
            }
            catch (BusinessException)
            {
                return View("Error");
            }

            return RedirectToAction("Show");
        }
    }
}
