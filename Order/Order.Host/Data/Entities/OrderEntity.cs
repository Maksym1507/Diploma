namespace Order.Host.Data.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Index { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public decimal TotalSum { get; set; }

        public List<OrderDetailsEntity> OrderDetails { get; set; } = new ();
    }
}
