using Microsoft.EntityFrameworkCore;
using Order.Host.Models;

namespace Order.Host.Repositories.Abstractions
{
    public interface IOrderItemRepository
    {
        Task<int?> AddOrderAsync(OrderEntity order, List<BasketItemModel> items);

        Task<OrderEntity?> GetByIdAsync(int id);

        Task<bool> DeleteOrderAsync(OrderEntity order);
    }
}
