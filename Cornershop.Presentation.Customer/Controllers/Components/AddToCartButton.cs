using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class AddToCartButton : ViewComponent
{
    public IViewComponentResult Invoke(string productId)
    {
        return View(new AddToCartModel { ProductId = productId, Quantity = 1 });
    }
}