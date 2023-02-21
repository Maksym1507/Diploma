namespace Basket.Host.Models
{
    public class CartItemModel
    {
        [Required]
        public ProductToBasketModel Product { get; set; } = null!;

        [Required]
        public int Count { get; set; }
    }
}
