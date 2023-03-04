using CatalogWebAPI.Data.Entities;

namespace WebAPI.Models.Responses
{
    // Responses for Company, Genre, Read
    public class ItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string PictureUrl { get; set; } = null!;
        public int AvailableStock { get; set; }

        public CatalogCompanyEntity CatalogCompany { get; set; } = null!;
        public CatalogGenreEntity CatalogGenre { get; set; } = null!;
    }
}
