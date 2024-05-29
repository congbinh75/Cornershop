using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CheckoutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
