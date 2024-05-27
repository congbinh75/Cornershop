using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class ProductController(ILogger<ProductController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
