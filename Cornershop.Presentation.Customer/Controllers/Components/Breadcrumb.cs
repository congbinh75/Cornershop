using Cornershop.Presentation.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared;
using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Components;

public class Breadcrumb : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}