using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.ObjectPool;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using System.ComponentModel;
using System.Text;

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
            int? pageIndex, int? itemsPerPage, int? companyFilter, int? genreFilter)
        {
            pageIndex ??= 0;
            itemsPerPage ??= 6;

            var catalog = await _catalogService.GetCatalogItems(
                pageIndex.Value, itemsPerPage.Value, companyFilter, genreFilter);

            if (catalog is null)
            {
                return View("Error");
            }

            var paginationInfo = new PaginationInfo()
            {
                TotalItems = catalog.Count,
                ItemsPerPage = itemsPerPage.Value,
                CurrentPage = pageIndex.Value,
                TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPerPage.Value)
            };

            var vm = new IndexViewModel()
            {
                CatalogItems = catalog.ItemsList,
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
        public async Task<IEnumerable<SelectListItem>> GetCompanies()
        {
            var companiesSelectList = await _catalogService.GetCompanies();

            return companiesSelectList!;
        }

        // Test method
        public async Task<IEnumerable<SelectListItem>> GetGenres()
        {
            var companiesSelectList = await _catalogService.GetGenres();

            return companiesSelectList!;
        }
    }
}
