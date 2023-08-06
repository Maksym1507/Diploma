namespace Basket.Host.Models
{
    public class ProductToBasketModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string PictureUrl { get; set; } = null!;
    }
}
