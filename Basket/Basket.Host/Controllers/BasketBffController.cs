using Basket.Host.Models;
using Basket.Host.Models.Requests;
using Basket.Host.Services.Abstractions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Basket.Host.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Scope("basket.basketbff")]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketBffController : ControllerBase
    {
        private readonly ILogger<BasketBffController> _logger;
        private readonly IBasketService _basketService;

        public BasketBffController(
            ILogger<BasketBffController> logger,
            IBasketService basketService)
        {
            _logger = logger;
            _basketService = basketService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketRequest request)
        {
            var response = await _basketService.AddItemToBasketAsync(request.Id, request.Product);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(GetBasketRequest request)
        {
            var response = await _basketService.GetBasketAsync(request.UserId);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteItemFromBasket(DeleteBasketItemRequest request)
        {
            var response = await _basketService.DeleteBasketItemAsync(request.UserId, request.BasketItemId);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TruncateBasket(TruncateBasketRequest request)
        {
            var response = await _basketService.TruncateBasketAsync(request.UserId);
            return Ok(response);
        }
    }
}