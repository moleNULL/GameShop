﻿namespace Basket.WebAPI.Models
{
    public class SetItemRequest
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal ItemPrice { get; set; }
        public string ItemPictureUrl { get; set; } = null!;
        public int ItemQuantity { get; set; }
    }
}
