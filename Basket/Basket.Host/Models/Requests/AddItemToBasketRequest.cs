namespace Basket.Host.Models.Requests
{
    public class AddItemToBasketRequest
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public ProductToBasketModel Product { get; set; } = null!;
    }
}
