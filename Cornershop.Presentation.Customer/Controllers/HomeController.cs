using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Presentation.Customer.Models;
using Cornershop.Presentation.Customer.Interfaces;

namespace Cornershop.Presentation.Controllers;

public class HomeController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var (products, count) = await productService.GetAll(1, 10);
        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
