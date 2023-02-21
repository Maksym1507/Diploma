namespace Basket.Host.Models.Responses
{
    public class GetResponse
    {
        [Required]
        public CartItemModel[] Data { get; set; } = null!;
    }
}
