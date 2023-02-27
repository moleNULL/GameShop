using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;

namespace MVC.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItems(int pageIndex, int itemsPerPage, int? companyFilter, int? genreFilter);
        Task<IEnumerable<SelectListItem>> GetCompanies();
        Task<IEnumerable<SelectListItem>> GetGenres();
    }
}
