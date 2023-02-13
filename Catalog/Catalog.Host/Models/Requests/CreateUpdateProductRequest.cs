namespace Catalog.Host.Models.Requests
{
    public class CreateUpdateProductRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public double Weight { get; set; }

        [Required]
        public string PictureFileName { get; set; } = null!;

        public int CatalogTypeId { get; set; }

        [Range(1, 100, ErrorMessage = "Invalid quantity")]
        public int AvailableStock { get; set; }
    }
}
