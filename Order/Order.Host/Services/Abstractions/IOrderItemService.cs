using Order.Host.Models;
using Order.Host.Models.Dtos;

namespace Order.Host.Services.Abstractions
{
    public interface IOrderItemService
    {
        Task<int?> AddAsync(string userId, string name, string lastName, BasketItemModel[] basketItems, string phoneNumber, string email, string country, string region, string city, string address, string index, decimal totalSum);

        Task<OrderDto?> GetOrderByIdAsync(int id);

        Task<bool> DeleteOrderAsync(int id);
    }
}
