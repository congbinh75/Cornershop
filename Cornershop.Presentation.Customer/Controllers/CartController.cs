using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CartController(ILogger<CartController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
