﻿namespace Basket.Host.Models.Requests
{
    public class GetBasketRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
    }
}
