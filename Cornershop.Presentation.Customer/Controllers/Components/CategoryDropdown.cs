using Cornershop.Presentation.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class CategoryDropdown(ICategoryService categoryService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await categoryService.GetAll(1, 128);
        return View(categories);
    }
}