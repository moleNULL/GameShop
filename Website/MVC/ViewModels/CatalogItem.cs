namespace MVC.ViewModels
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string PictureUrl { get; set; } = null!;
        public int AvailableStock { get; set; }

        public CatalogCompany CatalogCompany { get; set; } = null!;
        public CatalogGenre CatalogGenre { get; set; } = null!;
    }
}
