using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Models.Dtos;

namespace Order.Host.Controllers
{
    [ApiController]
    [Scope("order.orderitem")]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderItemController : ControllerBase
    {
        private readonly ILogger<OrderItemController> _logger;
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(ILogger<OrderItemController> logger, IOrderItemService orderItemService)
        {
            _logger = logger;
            _orderItemService = orderItemService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(AddOrderRequest request)
        {
            var result = await _orderItemService.AddAsync(request);
            return Ok(new AddItemResponse<int?>() { Id = result });
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemById(int id)
        {
            var result = await _orderItemService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(DeleteOrderItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderItemService.DeleteOrderAsync(id);
            return Ok(new DeleteOrderItemResponse<bool>() { IsDeleted = result });
        }
    }
}
