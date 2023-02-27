using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class ApplicationUser : IdentityUser
    {
        public string CardNumber { get; set; } = null!;
        public string SecurityNumber { get; set;} = null!;
        public string Expiration { get; set; } = null!;
        public string CardHolderName { get; set; } = null!;
        public int CardType { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string StateCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public double Latitude { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
    }
}
