namespace Basket.Host.Models.Requests
{
    public class AddRequest
    {
        [Required]
        public CartItemModel[] Data { get; set; } = null!;
    }
}
