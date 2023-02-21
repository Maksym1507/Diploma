using Basket.Host.Models;
using Basket.Host.Models.Responses;

namespace Basket.Host.Services.Abstractions
{
    public interface IBasketService
    {
        Task Add(string userId, CartItemModel[] data);

        Task<GetResponse> Get(string userId);

        Task<bool> Delete(string userId);
    }
}
