using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
