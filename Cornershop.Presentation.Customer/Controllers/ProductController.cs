using Cornershop.Presentation.Customer.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class ProductController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? keyword = null, string? categoryId = null, string? subcategoryId = null)
    {
        var (products, count) = await productService.GetAll(page, pageSize, keyword, categoryId, subcategoryId);
        ViewData["keyword"] = keyword;
        ViewData["pagesCount"] = count;
        ViewData["categoryId"] = categoryId;
        ViewData["subcategoryId"] = subcategoryId;
        return View(products);
    }

    public async Task<IActionResult> Detail(string id)
    {
        var product = await productService.GetById(id);
        return View(product);
    }
}
