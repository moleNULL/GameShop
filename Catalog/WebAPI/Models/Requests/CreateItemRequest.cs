using CatalogWebAPI.Data.Entities;

namespace WebAPI.Models.Requests
{
    public class CreateItemRequest
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string PictureFileName { get; set; } = null!;
        public int AvailableStock { get; set; }

        public int CatalogCompanyId { get; set; }
        public int CatalogGenreId { get; set; }
    }
}
