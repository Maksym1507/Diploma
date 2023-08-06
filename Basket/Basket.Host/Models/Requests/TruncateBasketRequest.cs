namespace Basket.Host.Models.Requests
{
    public class TruncateBasketRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
    }
}
