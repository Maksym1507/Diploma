namespace Catalog.Host.Services
{
    public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogService> _loggerService;

        public CatalogService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogItemRepository catalogItemRepository,
            IMapper mapper,
            ILogger<CatalogService> loggerService)
            : base(dbContextWrapper, logger)
        {
            _catalogItemRepository = catalogItemRepository;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters)
        {
            return await ExecuteSafeAsync(async () =>
            {
                int? typeFilter = null;

                if (filters != null)
                {
                    if (filters.TryGetValue(CatalogTypeFilter.Type, out var type))
                    {
                        typeFilter = type;
                    }
                }

                var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize, typeFilter);

                if (result.Data.Count() == 0)
                {
                    _loggerService.LogWarning($"Not founded catalog items on page = {pageIndex}, with page size = {pageSize} and with type = {typeFilter}");
                    return null;
                }

                return new PaginatedItemsResponse<CatalogItemDto>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
        }

        public async Task<CatalogItemDto?> GetCatalogItemByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogItemRepository.GetByIdAsync(id);

                if (result == null)
                {
                    _loggerService.LogWarning($"Not founded catalog item with Id = {id}");
                    return null;
                }

                return _mapper.Map<CatalogItemDto>(result);
            });
        }

        public async Task<ItemsResponse<CatalogItemDto>?> GetCatalogItemsByTypeAsync(string type)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogItemRepository.GetByTypeAsync(type);

                if (result == null)
                {
                    _loggerService.LogWarning($"Not founded catalog items with type = {type}");
                    return null;
                }

                return new ItemsResponse<CatalogItemDto>()
                {
                    Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList()
                };
            });
        }
    }
}
