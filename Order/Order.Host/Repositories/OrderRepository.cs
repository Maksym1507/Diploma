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

        public async Task<int?> AddOrderAsync(string userId, string name, string lastName, string phoneNumber, string email, string country, string region, string city, string address, string index, decimal totalSum, List<BasketItemModel> items)
        {
            var result = await _dbContext.AddAsync(new OrderEntity
            {
                UserId = userId,
                Name = name,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Country = country,
                Region = region,
                City = city,
                Address = address,
                Index = index,
                CreatedAt = DateTime.UtcNow.Date,
                TotalSum = totalSum
            });

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
