namespace WebAPI.Data
{
    public class PaginatedItems<T>
    {
        public int TotalCount { get; init; }

        public IEnumerable<T>? ItemList { get; init; }
    }
}
