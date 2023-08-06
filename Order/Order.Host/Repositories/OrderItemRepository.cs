using Microsoft.EntityFrameworkCore;
using Order.Host.Models;

namespace Order.Host.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(
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

        public async Task<OrderEntity?> GetByIdAsync(int id)
        {
            return await _dbContext.Orders.Include(i => i.OrderDetails).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> DeleteOrderAsync(OrderEntity order)
        {
            _dbContext.OrderDetails.RemoveRange(order.OrderDetails);

            var quantityOrderDetailsDeleted = await _dbContext.SaveChangesAsync();

            _dbContext.Orders.Remove(order);

            var quantityOrdersDeleted = await _dbContext.SaveChangesAsync();

            if (quantityOrderDetailsDeleted > 0 && quantityOrdersDeleted > 0)
            {
                return true;
            }

            return false;
        }
    }
}
