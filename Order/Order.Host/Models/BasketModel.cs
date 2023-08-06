namespace Order.Host.Models
{
    public class BasketModel
    {
        public List<BasketItemModel> BasketItems { get; set; } = new List<BasketItemModel>();

        public decimal TotalSum { get; set; }
    }
}
