namespace Catalog.Host.Models.Requests
{
    public class CreateUpdateProductRequest
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public double Weight { get; set; }

        public string PictureFileName { get; set; } = null!;

        public int CatalogTypeId { get; set; }

        public int AvailableStock { get; set; }
    }
}
