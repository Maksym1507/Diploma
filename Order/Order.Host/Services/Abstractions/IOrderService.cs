using Order.Host.Models;
using Order.Host.Models.Requests;

namespace Order.Host.Services.Abstractions
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId);

        Task<int?> DoOrderAsync(string userId, string name, string lastName, BasketItemModel[] basketItems, string phoneNumber, string email, string country, string region, string city, string address, string index, decimal totalSum);
    }
}
