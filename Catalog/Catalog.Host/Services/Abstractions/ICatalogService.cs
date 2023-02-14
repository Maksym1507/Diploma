namespace Catalog.Host.Services.Abstractions
{
    public interface ICatalogService
    {
        Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);

        Task<ItemsResponse<CatalogItemDto>?> GetCatalogItemsByTypeAsync(string type);

        Task<CatalogItemDto?> GetCatalogItemByIdAsync(int id);
    }
}
