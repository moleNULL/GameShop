using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Requests
{
    public class PaginatedItemsRequest
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
    }
}
