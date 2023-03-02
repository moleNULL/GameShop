namespace MVC.Models.BasketModels
{
    public class AddItemsToBasketRequest
    {
        public IEnumerable<BasketItemDto> Items { get; set; } = null!;
    }
}
