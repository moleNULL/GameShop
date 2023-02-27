namespace MVC.ViewModels
{
    public class Catalog
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public int Count { get; init; }
        public List<CatalogItem> ItemsList { get; init; } = null!;
    }
}
