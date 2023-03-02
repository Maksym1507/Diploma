using Microsoft.Extensions.Options;
using Order.Host.Configurations;

namespace Order.Host.Services
{
    public class OrderService : BaseDataService<ApplicationDbContext>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _loggerService;
        private readonly IOrderRepository _orderRepository;
        private readonly IInternalHttpClientService _httpClient;
        private readonly OrderConfig _config;

        public OrderService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IOrderRepository orderRepository,
            IInternalHttpClientService httpClient,
            ILogger<OrderService> loggerService,
            IOptions<OrderConfig> config,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _loggerService = loggerService;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<int?> DoOrderAsync(AddOrderRequest order)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var addedOrder = _mapper.Map<OrderEntity>(order);

                var orderId = await _orderRepository.AddOrderAsync(addedOrder, order.BasketItems.ToList());

                _loggerService.LogWarning($"Order with id = {orderId} has been added");
                return orderId;
            });
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            if (orders!.Count() == 0)
            {
                _loggerService.LogWarning($"Not founded orders with user id = {userId}");
                return null!;
            }

            return orders!.Select(s => _mapper.Map<OrderResponse>(s));
        }
    }
}
