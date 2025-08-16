using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Services;

namespace ProductCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _svc;
        public ProductsController(ProductService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _svc.GetProductsAsync();
                return Ok(list);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(429, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
