namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;
        private readonly IMapper _mapper;

        public CatalogTypeService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogTypeRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
            _mapper = mapper;
        }

        public async Task<ItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogTypeRepository.GetAsync();

                if (result.Data.Count() == 0)
                {
                    throw new Exception($"Types not found");
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
                return await _catalogTypeRepository.AddAsync(type);
            });
        }

        public async Task<bool> UpdateAsync(int id, string type)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var typeToUpdate = await _catalogTypeRepository.GetByIdAsync(id);

                if (typeToUpdate == null)
                {
                    return false;
                }

                typeToUpdate.Type = type;

                return await _catalogTypeRepository.UpdateAsync(typeToUpdate);
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var typeToDelete = await _catalogTypeRepository.GetByIdAsync(id);

                if (typeToDelete == null)
                {
                    return false;
                }

                return await _catalogTypeRepository.DeleteAsync(typeToDelete);
            });
        }
    }
}
