using Microsoft.Extensions.Options;
using Order.Host.Configurations;
using Order.Host.Models.Dtos;

namespace Order.Host.Services
{
    public class OrderItemService : BaseDataService<ApplicationDbContext>, IOrderItemService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderItemService> _loggerService;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IOrderItemRepository orderItemRepository,
            IInternalHttpClientService httpClient,
            ILogger<OrderItemService> loggerService,
            IOptions<OrderConfig> config,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _loggerService = loggerService;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(AddOrderRequest order)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var addedOrder = _mapper.Map<OrderEntity>(order);

                var orderId = await _orderItemRepository.AddOrderAsync(addedOrder, order.BasketItems.ToList());

                _loggerService.LogWarning($"Order with id = {orderId} has been added");
                return orderId;
            });
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _orderItemRepository.GetByIdAsync(id);

                if (result == null)
                {
                    _loggerService.LogWarning($"Not founded catalog item with Id = {id}");
                    return null;
                }

                return _mapper.Map<OrderDto>(result);
            });
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var orderToDelete = await _orderItemRepository.GetByIdAsync(id);

                if (orderToDelete == null)
                {
                    _loggerService.LogWarning($"Not founded order with Id = {id}");
                    return false;
                }

                var isDeleted = await _orderItemRepository.DeleteOrderAsync(orderToDelete!);
                _loggerService.LogInformation($"Removed order with Id = {id}");
                return isDeleted;
            });
        }
    }
}
