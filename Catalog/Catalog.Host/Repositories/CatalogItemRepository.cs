namespace Catalog.Host.Repositories
{
    public class CatalogItemRepository : ICatalogItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogItemRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? typeFilter)
        {
            IQueryable<CatalogItem> query = _dbContext.CatalogItems;

            if (typeFilter.HasValue)
            {
                query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
            }

            var totalItems = await query.LongCountAsync();

            var itemsOnPage = await query.OrderBy(c => c.Title)
               .Include(i => i.CatalogType)
               .Skip(pageSize * pageIndex)
               .Take(pageSize)
               .ToListAsync();

            return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<Items<CatalogItem>> GetByTypeAsync(string type)
        {
            var result = await _dbContext.CatalogItems
                .Include(i => i.CatalogType).Where(w => w.CatalogType!.Type == type)
                .ToListAsync();

            return new Items<CatalogItem>() { Data = result };
        }

        public async Task<CatalogItem?> GetByIdAsync(int id)
        {
            return await _dbContext.CatalogItems.Include(i => i.CatalogType).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<int?> AddAsync(string title, string description, decimal price, double weight, int availableStock, int catalogTypeId, string pictureFileName)
        {
            var item = await _dbContext.AddAsync(new CatalogItem
            {
                CatalogTypeId = catalogTypeId,
                Description = description,
                Title = title,
                PictureFileName = pictureFileName,
                Price = price,
                Weight = weight,
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<bool> UpdateAsync(CatalogItem item)
        {
            _dbContext.CatalogItems.Update(item);

            var quantityCatalogItemsUpdated = await _dbContext.SaveChangesAsync();

            if (quantityCatalogItemsUpdated > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(CatalogItem item)
        {
            _dbContext.CatalogItems.Remove(item);

            var quantityCatalogItemsRemoved = await _dbContext.SaveChangesAsync();

            if (quantityCatalogItemsRemoved > 0)
            {
                return true;
            }

            return false;
        }
    }
}
