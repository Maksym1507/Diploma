namespace Catalog.Host.Repositories
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<Items<CatalogType>> GetAsync()
        {
            var result = await _dbContext.CatalogTypes
                .ToListAsync();

            return new Items<CatalogType>() { Data = result };
        }

        public async Task<CatalogType?> GetByIdAsync(int id)
        {
            return await _dbContext.CatalogTypes.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<int?> AddAsync(string type)
        {
            var item = await _dbContext.AddAsync(new CatalogType
            {
                Type = type
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<bool> UpdateAsync(CatalogType type)
        {
            _dbContext.CatalogTypes.Update(type);

            var quantityCatalogTypesUpdated = await _dbContext.SaveChangesAsync();

            if (quantityCatalogTypesUpdated > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(CatalogType type)
        {
            _dbContext.CatalogTypes.Remove(type);

            var quantityCatalogTypesRemoved = await _dbContext.SaveChangesAsync();

            if (quantityCatalogTypesRemoved > 0)
            {
                return true;
            }

            return false;
        }
    }
}
