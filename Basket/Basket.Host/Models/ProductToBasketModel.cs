namespace Basket.Host.Models
{
    public class ProductToBasketModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public double Weight { get; set; }

        [Required]
        public string PictureUrl { get; set; } = null!;

        public int CatalogTypeId { get; set; }

        public int AvailableStock { get; set; }
    }
}
