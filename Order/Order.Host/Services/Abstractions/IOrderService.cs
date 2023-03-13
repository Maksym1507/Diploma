namespace Order.Host.Services.Abstractions
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId);

        Task<int?> DoOrderAsync(string userId, string name, string lastName, string phoneNumber, string email, string country, string region, string city, string address, string index);
    }
}
