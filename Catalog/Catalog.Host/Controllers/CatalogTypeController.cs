using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("catalog.catalogtype")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogTypeController : ControllerBase
    {
        private readonly ICatalogTypeService _catalogTypeService;
        private readonly ILogger<CatalogTypeController> _logger;

        public CatalogTypeController(
            ICatalogTypeService catalogTypeService,
            ILogger<CatalogTypeController> logger)
        {
            _catalogTypeService = catalogTypeService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(CreateUpdateTypeRequest request)
        {
            var result = await _catalogTypeService.AddAsync(request.Type);
            return Ok(new AddItemResponse<int?>() { Id = result });
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(UpdateItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(int id, CreateUpdateTypeRequest request)
        {
            var result = await _catalogTypeService.UpdateAsync(id, request.Type);
            return Ok(new UpdateItemResponse<bool>() { IsUpdated = result });
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(DeleteItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _catalogTypeService.DeleteAsync(id);
            return Ok(new DeleteItemResponse<bool>() { IsDeleted = result });
        }
    }
}
