using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Requests
{
    public class ItemByGenreRequest
    {
        /// <summary>
        /// Gets or sets the genre of the item
        /// </summary>
        /// <example>RPG</example>
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        public string Genre { get; set; } = null!;
    }
}
