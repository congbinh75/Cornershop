using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Models;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class CategoryFilter(ICategoryService categoryService, ISubcategoryService subcategoryService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string? categoryId = null)
    {
        var categories = await categoryService.GetAll(1, 128);
        ICollection<SubcategoryDTO> subcategories = [];
        if (categoryId != null) subcategories = await subcategoryService.GetAllByCategory(categoryId, 1, 128);
        return View(new CategoryFilterModel { Categories = categories, Subcategories = subcategories });
    }
}