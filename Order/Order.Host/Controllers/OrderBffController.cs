using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Order.Host.Controllers
{
    [ApiController]
    /*[Scope("order.orderbff")]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]*/
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderBffController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderBffController> _logger;
        private readonly IOrderService _orderService;

        public OrderBffController(
            ILogger<OrderBffController> logger,
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            IOrderService orderService)
        {
            _logger = logger;
            _dbContext = dbContextWrapper.DbContext;
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DoOrder(AddOrderRequest order)
        {
            var result = await _orderService.DoOrderAsync(order);
            return Ok(result);
        }
    }
}