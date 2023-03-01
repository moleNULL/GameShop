namespace WebAPI.Models.Responses
{
    public class RemoveItemResponse<T>
    {
        public T IsRemoved { get; set; } = default!;
    }
}
