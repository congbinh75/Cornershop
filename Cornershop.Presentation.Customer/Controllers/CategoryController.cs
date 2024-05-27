using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CategoryController(ILogger<CategoryController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
