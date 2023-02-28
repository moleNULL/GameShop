using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Requests
{
    public class PaginatedItemsRequest<T>
        where T : notnull
    {
        /// <summary>
        /// Gets or sets the pageIndex of the item
        /// </summary>
        /// <example>0</example>
        [Range(minimum: 0, maximum: int.MaxValue)]
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the pageSize of the item
        /// </summary>
        /// <example>6</example>
        [Range(minimum: 0, maximum: int.MaxValue)]
        public int PageSize { get; set; }

        public Dictionary<T, int?> Filters { get; set; } = null!;
    }
}
