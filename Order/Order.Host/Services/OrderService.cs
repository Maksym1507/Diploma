using Microsoft.Extensions.Options;
using Order.Host.Configurations;
using Order.Host.Models;

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
            ILogger<OrderService> loggerService,
            IMapper mapper,
            IInternalHttpClientService httpClient,
            IOptions<OrderConfig> config)
            : base(dbContextWrapper, logger)
        {
            _loggerService = loggerService;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<int?> DoOrderAsync(string userId, string name, string lastName, string phoneNumber, string email, string country, string region, string city, string address, string index)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var basket = await _httpClient.SendAsync<BasketModel, GetBasketRequest>(
                $"{_config.BasketUrl}/get",
                HttpMethod.Post,
                new GetBasketRequest
                {
                    UserId = userId
                });

                if (basket.BasketItems.Count == 0)
                {
                    _loggerService.LogError($"Basket with id = {userId} is empty");
                    return 0;
                }

                var orderId = await _orderRepository.AddOrderAsync(userId, name, lastName, phoneNumber, email, country, region, city, address, index, basket.TotalSum, basket.BasketItems);

                if (orderId == 0)
                {
                    _loggerService.LogError("Failed add order to db");
                    return 0;
                }

                _loggerService.LogInformation($"Order with id = {orderId} has been added");
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
