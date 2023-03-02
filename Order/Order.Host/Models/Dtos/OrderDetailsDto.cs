namespace Order.Host.Models.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int CatalogItemId { get; set; }

        public int Count { get; set; }
    }
}
