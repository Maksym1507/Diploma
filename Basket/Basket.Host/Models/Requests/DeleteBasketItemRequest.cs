namespace Basket.Host.Models.Requests
{
    public class DeleteBasketItemRequest
    {
        [Required]
        public string UserId { get; set; } = null!;

        public int BasketItemId { get; set; }
    }
}
