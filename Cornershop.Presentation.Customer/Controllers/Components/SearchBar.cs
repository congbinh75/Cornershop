using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class SearchBar : ViewComponent
{
    public IViewComponentResult Invoke(string? keyword = null)
    {
        ViewData["keyword"] = keyword ?? string.Empty;
        return View();
    }
}