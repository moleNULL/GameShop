namespace WebAPI.Models.Responses
{
    public class AddItemResponse<T>
    {
        public T Item { get; set; } = default!;
    }
}
