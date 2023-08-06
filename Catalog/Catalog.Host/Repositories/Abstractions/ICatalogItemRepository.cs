namespace Catalog.Host.Repositories.Abstractions
{
    public interface ICatalogItemRepository
    {
        Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? typeFilter);

        Task<CatalogItem?> GetByIdAsync(int id);

        Task<Items<CatalogItem>> GetByTypeAsync(string type);

        Task<int?> AddAsync(string title, string description, decimal price, double weight, int availableStock, int catalogTypeId, string pictureFileName);

        Task<bool> UpdateAsync(CatalogItem item);

        Task<bool> DeleteAsync(CatalogItem item);
    }
}
