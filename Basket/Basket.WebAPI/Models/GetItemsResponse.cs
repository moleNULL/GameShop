using System.ComponentModel.DataAnnotations;

namespace Basket.WebAPI.Models
{
    public class GetItemsResponse
    {
        [Required]
        public IEnumerable<ItemDto> Items { get; init; } = null!;
    }
}
