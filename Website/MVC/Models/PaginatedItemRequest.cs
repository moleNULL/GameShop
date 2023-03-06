#nullable disable

namespace MVC.Models
{
    public class PaginatedItemRequest<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public Dictionary<T, int?> Filters { get; set; }
    }
}
