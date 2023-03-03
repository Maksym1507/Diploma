using Microsoft.EntityFrameworkCore;
using Order.Host.Models;

namespace Order.Host.Repositories.Abstractions
{
    public interface IOrderItemRepository
    {
        Task<int?> AddOrderAsync(string userId, string name, string lastName, string phoneNumber, string email, string country, string region, string city, string address, string index, decimal totalSum, List<BasketItemModel> items);

        Task<OrderEntity?> GetByIdAsync(int id);

        Task<bool> DeleteOrderAsync(OrderEntity order);
    }
}
