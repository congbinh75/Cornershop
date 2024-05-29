using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
