using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CartController(ICartService cartService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var cart = await cartService.GetCartByCurrentUser();
        return View(cart);
    }

    public async Task<IActionResult> AddItem(AddToCartModel model)
    {
        var cart = await cartService.AddItem(model.ProductId, model.Quantity);
        return RedirectToAction("Index", "Cart", new { cart });
    }

    public async Task<IActionResult> RemoveItem(string productId, int quantity)
    {
        var cart = await cartService.RemoveItem(productId, quantity);
        return RedirectToAction("Index", "Cart", new { cart });
    }
}
