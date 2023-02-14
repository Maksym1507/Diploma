using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogBffController : ControllerBase
    {
        private readonly ILogger<CatalogBffController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogTypeService _catalogTypeService;

        public CatalogBffController(
            ILogger<CatalogBffController> logger,
            ICatalogService catalogService,
            ICatalogTypeService catalogTypeService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _catalogTypeService = catalogTypeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
        {
            var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemsByType(ItemsByTypeRequest request)
        {
            var result = await _catalogService.GetCatalogItemsByTypeAsync(request.Type);
            return Ok(result);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(CatalogItemDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemById(int id)
        {
            var result = await _catalogService.GetCatalogItemByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemsResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Types()
        {
            var result = await _catalogTypeService.GetCatalogTypesAsync();
            return Ok(result);
        }
    }
}
