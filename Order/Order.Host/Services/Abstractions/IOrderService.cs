using Order.Host.Models.Requests;

namespace Order.Host.Services.Abstractions
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId);

        Task<int?> DoOrderAsync(AddOrderRequest order);
    }
}
