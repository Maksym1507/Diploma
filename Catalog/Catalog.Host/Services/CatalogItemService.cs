namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ILogger<CatalogItemService> _loggerService;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        ILogger<CatalogItemService> loggerService)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _loggerService = loggerService;
    }

    public async Task<int?> AddAsync(string title, string description, decimal price, double weight, int availableStock, int catalogTypeId, string pictureFileName)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var id = await _catalogItemRepository.AddAsync(title, description, price, weight, availableStock, catalogTypeId, pictureFileName);
            _loggerService.LogInformation($"Created catalog item with Id = {id}");
            return id;
        });
    }

    public async Task<bool> UpdateAsync(int id, string title, string description, decimal price, double weight, int availableStock, int catalogTypeId, string pictureFileName)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var itemToUpdate = await _catalogItemRepository.GetByIdAsync(id);

            if (itemToUpdate == null)
            {
                _loggerService.LogWarning($"Not founded catalog item with Id = {id}");
                return false;
            }

            itemToUpdate.Title = title;
            itemToUpdate.Description = description;
            itemToUpdate.Price = price;
            itemToUpdate.Weight = weight;
            itemToUpdate.AvailableStock = availableStock;
            itemToUpdate.CatalogTypeId = catalogTypeId;
            itemToUpdate.PictureFileName = pictureFileName;

            var isUpdated = await _catalogItemRepository.UpdateAsync(itemToUpdate);
            _loggerService.LogInformation($"Updated catalog item with Id = {id}");
            return isUpdated;
        });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var itemToDelete = await _catalogItemRepository.GetByIdAsync(id);

            if (itemToDelete == null)
            {
                _loggerService.LogWarning($"Not founded catalog item with Id = {id}");
                return false;
            }

            var isDeleted = await _catalogItemRepository.DeleteAsync(itemToDelete);
            _loggerService.LogInformation($"Removed catalog item with Id = {id}");
            return isDeleted;
        });
    }
}
