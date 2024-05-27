using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CheckoutController(ILogger<CheckoutController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
