namespace Order.Host.Models.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public OrderDetailsResponse[] OrderProducts { get; set; } = null!;

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
    }
}
