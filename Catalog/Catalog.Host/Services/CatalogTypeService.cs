namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogTypeService> _loggerService;

        public CatalogTypeService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogTypeRepository,
            IMapper mapper,
            ILogger<CatalogTypeService> loggerService)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        public async Task<ItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogTypeRepository.GetAsync();

                if (result.Data.Count() == 0)
                {
                    _loggerService.LogWarning("No catalog types in database");
                    return null;
                }

                return new ItemsResponse<CatalogTypeDto>()
                {
                    Data = result.Data.Select(s => _mapper.Map<CatalogTypeDto>(s)).ToList()
                };
            });
        }

        public async Task<int?> AddAsync(string type)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var id = await _catalogTypeRepository.AddAsync(type);
                _loggerService.LogInformation($"Created catalog type with Id = {id}");
                return id;
            });
        }

        public async Task<bool> UpdateAsync(int id, string type)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var typeToUpdate = await _catalogTypeRepository.GetByIdAsync(id);

                if (typeToUpdate == null)
                {
                    _loggerService.LogWarning($"Not founded catalog type with Id = {id}");
                    return false;
                }

                typeToUpdate.Type = type;

                var isUpdated = await _catalogTypeRepository.UpdateAsync(typeToUpdate);
                _loggerService.LogInformation($"Updated catalog type with Id = {id}");
                return isUpdated;
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var typeToDelete = await _catalogTypeRepository.GetByIdAsync(id);

                if (typeToDelete == null)
                {
                    _loggerService.LogWarning($"Not founded catalog type with Id = {id}");
                    return false;
                }

                var isDeleted = await _catalogTypeRepository.DeleteAsync(typeToDelete);
                _loggerService.LogInformation($"Removed catalog type with Id = {id}");
                return isDeleted;
            });
        }
    }
}
