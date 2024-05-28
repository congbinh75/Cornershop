using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Presentation.Models;
using Cornershop.Presentation.Customer.Intefaces;

namespace Cornershop.Presentation.Controllers;

public class HomeController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await productService.GetAll(1, 10);
        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
