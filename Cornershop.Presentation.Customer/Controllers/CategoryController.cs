using Cornershop.Presentation.Customer.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CategoryController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
    {
        var products = await productService.GetAll(page, pageSize);
        return View(products);
    }
}
