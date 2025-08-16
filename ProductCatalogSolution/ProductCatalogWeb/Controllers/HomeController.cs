using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogWeb.Models;

namespace ProductCatalogWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Products");

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var vm = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        return View(vm);
    }
}
