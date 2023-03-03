namespace Order.Host.Models.Requests
{
    public class AddOrderRequest
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public BasketItemModel[] BasketItems { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Incorrent email format")]
        public string Email { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string Region { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string Index { get; set; } = null!;

        public decimal TotalSum { get; set; }
    }
}
