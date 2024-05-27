using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;

    public CartController(ILogger<CartController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
