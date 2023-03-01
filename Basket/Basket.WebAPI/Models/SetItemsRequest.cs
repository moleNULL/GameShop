using System.ComponentModel.DataAnnotations;

namespace Basket.WebAPI.Models
{
    public class SetItemsRequest
    {
        [Required]
        public IEnumerable<ItemDto> Items { get; set; } = null!;
    }
}
