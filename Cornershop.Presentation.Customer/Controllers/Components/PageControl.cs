using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class PageControl : ViewComponent
{
    public IViewComponentResult Invoke(int? pagesCount = null)
    {
        ViewData["keyword"] = pagesCount ?? 1;
        return View();
    }
}