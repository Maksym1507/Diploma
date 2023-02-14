namespace Catalog.Host.Data.Entities
{
    public class CatalogItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; }

        public string PictureFileName { get; set; }

        public decimal Price { get; set; }

        public double Weight { get; set; }

        public int CatalogTypeId { get; set; }

        public CatalogType? CatalogType { get; set; }

        public int AvailableStock { get; set; }
    }
}
