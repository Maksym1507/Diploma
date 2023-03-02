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
