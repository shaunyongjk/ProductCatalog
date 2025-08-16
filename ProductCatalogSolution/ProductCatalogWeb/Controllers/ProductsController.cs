using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogWeb.Services;

namespace ProductCatalogWeb.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _service;

    public ProductsController(IProductService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var products = await _service.GetProductsAsync();
        return View(products);
    }
}
