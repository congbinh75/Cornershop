using Cornershop.Presentation.Customer.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class ProductController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? categoryId = null, string? subcategoryId = null)
    {
        var products = await productService.GetAll(page, pageSize, categoryId, subcategoryId);
        return View(products);
    }

    public async Task<IActionResult> Detail(string id)
    {
        var product = await productService.GetById(id);
        return View(product);
    }
}
