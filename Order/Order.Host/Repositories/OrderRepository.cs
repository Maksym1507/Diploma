using Order.Host.Models;

namespace Order.Host.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        {
            _dbContext = dbContextWrapper.DbContext;
        }

        public async Task<int?> AddOrderAsync(OrderEntity order, List<BasketItemModel> items)
        {
            var a = _dbContext.Orders.Count();
            var b = _dbContext.OrderDetails.Count();
            var result = await _dbContext.AddAsync(order);

            await _dbContext.OrderDetails.AddRangeAsync(items.Select(s => new OrderDetailsEntity()
            {
                OrderId = result.Entity.Id,
                CatalogItemId = s.Id,
                Count = s.Count
            }));

            await _dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<IEnumerable<OrderEntity>?> GetOrdersByUserIdAsync(string userId)
        {
            return await _dbContext.Orders.Include(i => i.OrderDetails).Where(f => f.UserId == userId).ToListAsync();
        }
    }
}
