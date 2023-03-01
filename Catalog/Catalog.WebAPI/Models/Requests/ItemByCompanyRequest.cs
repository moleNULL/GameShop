using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Requests
{
    public class ItemByCompanyRequest
    {
        /// <summary>
        /// Gets or sets the company of the item
        /// </summary>
        /// <example>Valve</example>
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        public string Company { get; set; } = null!;
    }
}
