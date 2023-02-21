using Basket.Host.Models;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Abstractions;

namespace Basket.Host.Services
{
    public class BasketService : IBasketService
    {
        private readonly ICacheService _cacheService;

        public BasketService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task Add(string userId, CartItemModel[] data)
        {
            await _cacheService.AddOrUpdateAsync(userId, data);
        }

        public async Task<GetResponse> Get(string userId)
        {
            var result = await _cacheService.GetAsync<CartItemModel[]>(userId);
            return new GetResponse() { Data = result };
        }

        public async Task<bool> Delete(string userId)
        {
            return await _cacheService.RemoveAsync(userId);
        }
    }
}
