namespace MVC.Models.BasketModels
{
    public class GetItemsFromBasketResponse
    {
        public IEnumerable<BasketItemDto> Items { get; init; } = null!;
    }
}
