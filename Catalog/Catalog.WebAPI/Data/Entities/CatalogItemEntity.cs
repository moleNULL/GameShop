namespace CatalogWebAPI.Data.Entities
{
    public class CatalogItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string PictureFileName { get; set; } = null!;
        public int AvailableStock { get; set; }

        public int CatalogCompanyId { get; set; }
        public CatalogCompanyEntity CatalogCompany { get; set; } = null!;
        public int CatalogGenreId { get; set; }
        public CatalogGenreEntity CatalogGenre { get; set; } = null!;
    }
}
