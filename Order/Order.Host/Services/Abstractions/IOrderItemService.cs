using Order.Host.Models.Dtos;

namespace Order.Host.Services.Abstractions
{
    public interface IOrderItemService
    {
        Task<int?> AddAsync(AddOrderRequest order);

        Task<OrderDto?> GetOrderByIdAsync(int id);

        Task<bool> DeleteOrderAsync(int id);
    }
}
