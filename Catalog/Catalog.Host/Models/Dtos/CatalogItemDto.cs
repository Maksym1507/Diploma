namespace Catalog.Host.Models.Dtos
{
    public class CatalogItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public double Weight { get; set; }

        public string PictureUrl { get; set; }

        public CatalogTypeDto CatalogType { get; set; }

        public int AvailableStock { get; set; }
    }
}
