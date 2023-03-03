namespace Order.Host.Models
{
    public class CatalogItemModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public double Weight { get; set; }

        public string PictureUrl { get; set; } = null!;

        public int CatalogTypeId { get; set; }

        public int AvailableStock { get; set; }
    }
}
