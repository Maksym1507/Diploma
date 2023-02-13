namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task<int?> AddAsync(string title, string description, decimal price, double weight, int availableStock, int catalogTypeId, string pictureFileName)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _catalogItemRepository.AddAsync(title, description, price, weight, availableStock, catalogTypeId, pictureFileName);
        });
    }

    public async Task<bool> UpdateAsync(int id, string title, string description, decimal price, double weight, int availableStock, int catalogTypeId, string pictureFileName)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var itemToUpdate = await _catalogItemRepository.GetByIdAsync(id);

            if (itemToUpdate == null)
            {
                return false;
            }

            itemToUpdate.Title = title;
            itemToUpdate.Description = description;
            itemToUpdate.Price = price;
            itemToUpdate.Weight = weight;
            itemToUpdate.AvailableStock = availableStock;
            itemToUpdate.CatalogTypeId = catalogTypeId;
            itemToUpdate.PictureFileName = pictureFileName;

            return await _catalogItemRepository.UpdateAsync(itemToUpdate);
        });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var itemToDelete = await _catalogItemRepository.GetByIdAsync(id);

            if (itemToDelete == null)
            {
                return false;
            }

            return await _catalogItemRepository.DeleteAsync(itemToDelete);
        });
    }
}
