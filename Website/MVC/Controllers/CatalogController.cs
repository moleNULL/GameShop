using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Services.Interfaces;
using MVC.ViewModels;

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

            var catalog = await _catalogService.GetCatalogItems(
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
                Companies = await _catalogService.GetCompanies(),
                Genres = await _catalogService.GetGenres(),
                PaginationInfo = paginationInfo
            };

            vm.PaginationInfo.NextPage =
                (vm.PaginationInfo.CurrentPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.PreviousPage =
                (vm.PaginationInfo.CurrentPage == 0) ? "is-disabled" : "";

            return View(vm);
        }

        // Test method
        public async Task<IEnumerable<SelectListItem>> Co()
        {
            var companiesSelectList = await _catalogService.GetCompanies();

            return companiesSelectList!;
        }

        // Test method
        public async Task<IEnumerable<SelectListItem>> Ge()
        {
            var companiesSelectList = await _catalogService.GetGenres();

            return companiesSelectList!;
        }
    }
}
