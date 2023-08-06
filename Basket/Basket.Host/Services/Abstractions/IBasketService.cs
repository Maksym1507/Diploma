using Basket.Host.Models;
namespace Basket.Host.Services.Abstractions
{
    public interface IBasketService
    {
        Task<bool> AddItemToBasketAsync(string userId, ProductToBasketModel data);

        Task<BasketModel> GetBasketAsync(string userId);

        Task<bool> DeleteBasketItemAsync(string userId, int basketItemId);

        Task<bool> TruncateBasketAsync(string userId);
    }
}
