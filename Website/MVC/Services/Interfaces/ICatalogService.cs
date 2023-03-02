using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models.BasketModels;
using MVC.Models.CatalogDtos;
using MVC.ViewModels;

namespace MVC.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItemsAsync(int pageIndex, int itemsPerPage, int? companyFilter, int? genreFilter);
        Task<IEnumerable<SelectListItem>> GetCompaniesAsync();
        Task<IEnumerable<SelectListItem>> GetGenresAsync();
        Task<IEnumerable<CatalogItemDto>> GetAllCatalogItemsAsync();
        Task<bool> AddItemsToBasketAsync(IEnumerable<BasketItemDto> items);
        Task<IEnumerable<BasketItemDto>> GetItemsFromBasketAsync();
    }
}
