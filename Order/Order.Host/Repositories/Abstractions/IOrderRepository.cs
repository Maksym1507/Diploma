using Order.Host.Models;

namespace Order.Host.Repositories.Abstractions
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderEntity>?> GetOrdersByUserIdAsync(string userId);

        Task<int?> AddOrderAsync(OrderEntity order, List<BasketItemModel> items);
    }
}
