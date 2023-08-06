using Order.Host.Models;
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
            ILogger<OrderItemService> loggerService,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _loggerService = loggerService;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(string userId, string name, string lastName, BasketItemModel[] basketItems, string phoneNumber, string email, string country, string region, string city, string address, string index, decimal totalSum)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var orderId = await _orderItemRepository.AddOrderAsync(userId, name, lastName, phoneNumber, email, country, region, city, address, index, totalSum, basketItems.ToList());

                if (orderId == 0)
                {
                    _loggerService.LogError("Failed add order to db");
                    return 0;
                }

                _loggerService.LogInformation($"Order with id = {orderId} has been added");
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
                    _loggerService.LogWarning($"Not founded order item with Id = {id}");
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
