using Order.Host.Models;

namespace Order.Host.Repositories.Abstractions
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderEntity>?> GetOrdersByUserIdAsync(string userId);

        Task<int?> AddOrderAsync(string userId, string name, string lastName, string phoneNumber, string email, string country, string region, string city, string address, string index, decimal totalSum, List<BasketItemModel> items);
    }
}
