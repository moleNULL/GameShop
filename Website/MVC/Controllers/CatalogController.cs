using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models.BasketModels;
using MVC.Models.CatalogDtos;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index(
            int? page, int? itemsPage, int? companyFilter, int? genreFilter)
        {
            page ??= 0;
            itemsPage ??= 6;

            var catalog = await _catalogService.GetCatalogItemsAsync(
                page.Value, itemsPage.Value, companyFilter, genreFilter);

            if (catalog is null)
            {
                return View("Error");
            }

            var paginationInfo = new PaginationInfo()
            {
                TotalItems = catalog.Count,
                ItemsPerPage = itemsPage.Value,
                CurrentPage = page.Value,
                TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
            };

            var vm = new IndexViewModel()
            {
                CatalogItems = catalog.ItemList,
                Companies = await _catalogService.GetCompaniesAsync(),
                Genres = await _catalogService.GetGenresAsync(),
                PaginationInfo = paginationInfo
            };

            vm.PaginationInfo.NextPage =
                (vm.PaginationInfo.CurrentPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.PreviousPage =
                (vm.PaginationInfo.CurrentPage == 0) ? "is-disabled" : "";

            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BasketAddAsync()
        {
            var catalogItems = await _catalogService.GetAllCatalogItemsAsync() ?? new List<CatalogItemDto>();

            return View(catalogItems);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BasketAddAsync(List<string> items)
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
                bool isAdded = await _catalogService.AddItemsToBasketAsync(basketItems);
                ViewData["isAdded"] = isAdded;
            }

            var catalogItems = await _catalogService.GetAllCatalogItemsAsync() ?? new List<CatalogItemDto>();

            return View(catalogItems);
        }

        [Authorize]
        public async Task<IActionResult> BasketShowAsync()
        {
            var basketItems = await _catalogService.GetItemsFromBasketAsync() ?? new List<BasketItemDto>();

            return View(basketItems);
        }

        [Authorize]
        public async Task<IActionResult> BasketEmptyAsync()
        {
            await _catalogService.EmptyBasketAsync();

            return RedirectToAction("BasketShow", "Catalog");
        }
    }
}
