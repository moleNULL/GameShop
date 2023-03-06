using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Requests
{
    public class ItemByIdRequest
    {
        /// <summary>
        /// Gets or sets the id of the item
        /// </summary>
        /// <example>1</example>
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Id { get; set; }
    }
}
