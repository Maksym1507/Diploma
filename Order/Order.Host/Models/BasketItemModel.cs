namespace Order.Host.Models
{
    public class BasketItemModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public int Count { get; set; }
    }
}
