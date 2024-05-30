using Cornershop.Presentation.Customer.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared;
using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Components;

public class Breadcrumb : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}