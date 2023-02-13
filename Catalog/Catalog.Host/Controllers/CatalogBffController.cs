using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogBffController : ControllerBase
    {
        private readonly ILogger<CatalogBffController> _logger;

        public CatalogBffController(ILogger<CatalogBffController> logger)
        {
            _logger = logger;
        }
    }
}