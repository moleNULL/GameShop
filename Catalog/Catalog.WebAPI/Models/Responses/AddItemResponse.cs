namespace WebAPI.Models.Responses
{
    public class AddItemResponse<T>
    {
        public T ItemId { get; set; } = default!;
    }
}
