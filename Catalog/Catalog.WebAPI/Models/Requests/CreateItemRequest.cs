using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Requests
{
    public class CreateItemRequest
    {
        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        /// <example>ExampleGame</example>
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the price of the item
        /// </summary>
        /// <example>0,01</example>
        // maxValue cuz we can change currency to any we want
        [Range(minimum: 0.01, maximum: double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the year of the item
        /// </summary>
        /// <example>2003</example>
        [Range(minimum: 2003, maximum: 2023)]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the pictureFileName of the item
        /// </summary>
        /// <example>example.png</example>
        [RegularExpression(pattern: @"^.+\.((jpg)|(png))$", ErrorMessage = "Picture file must be in JPG or PNG format")]
        public string PictureFileName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the availableStock of the item
        /// </summary>
        /// <example>0</example>
        [Range(minimum: 0, maximum: int.MaxValue)]
        public int AvailableStock { get; set; }

        /// <summary>
        /// Gets or sets the catalogCompanyId of the item
        /// </summary>
        /// <example>1</example>
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int CatalogCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the catalogGenreId of the item
        /// </summary>
        /// <example>1</example>
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int CatalogGenreId { get; set; }
    }
}
