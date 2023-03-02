namespace Order.Host.Models.Requests
{
    public class CartItemRequest
    {
        public int CatalogItemId { get; set; }

        public int Count { get; set; }
    }
}
