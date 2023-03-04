using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _basketService;

        public CatalogController(ICatalogService catalogService)
        {
            _basketService = catalogService;
        }

        public async Task<IActionResult> Index(
            int? page, int? itemsPage, int? companyFilter, int? genreFilter)
        {
            page ??= 0;
            itemsPage ??= 6;

            var catalog = await _basketService.GetCatalogItemsAsync(
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
                Companies = await _basketService.GetCompaniesAsync(),
                Genres = await _basketService.GetGenresAsync(),
                PaginationInfo = paginationInfo
            };

            vm.PaginationInfo.NextPage =
                (vm.PaginationInfo.CurrentPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.PreviousPage =
                (vm.PaginationInfo.CurrentPage == 0) ? "is-disabled" : "";

            return View(vm);
        }
    }
}
