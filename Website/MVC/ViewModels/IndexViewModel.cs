using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<CatalogItem> CatalogItems { get; set; } = null!;
        public IEnumerable<SelectListItem> Companies { get; set; } = null!;
        public IEnumerable<SelectListItem> Genres { get; set; } = null!;

        public int? CompanyFilter { get; set; }
        public int? GenreFilter { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = null!;
    }
}
