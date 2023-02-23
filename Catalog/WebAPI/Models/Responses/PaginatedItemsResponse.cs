namespace WebAPI.Models.Responses
{
    public class PaginatedItemsResponse<T>
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }

        public int Count { get; init; }
        public IEnumerable<T>? ItemList { get; init; }
    }
}
