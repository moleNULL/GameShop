using System.ComponentModel.DataAnnotations;
using CatalogWebAPI.Data.Entities;

namespace WebAPI.Models.Dtos
{
    public class CatalogItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string PictureUrl { get; set; } = null!;
        public int AvailableStock { get; set; }

        public CatalogCompanyDto CatalogCompany { get; set; } = null!;
        public CatalogGenreDto CatalogGenre { get; set; } = null!;
    }
}
