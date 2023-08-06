namespace Order.Host.Models.Requests
{
    public class BasketItemRequest
    {
        public int CatalogItemId { get; set; }

        public int Count { get; set; }
    }
}
