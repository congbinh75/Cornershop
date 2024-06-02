using Cornershop.Presentation.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class CartController(ICartService cartService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var cart = await cartService.GetCartByCurrentUser();
        return View(cart);
    }

    public async Task<IActionResult> AddItem(string productId, int quantity)
    {
        var cart = await cartService.AddItem(productId, quantity);
        return View(cart);
    }

    public async Task<IActionResult> RemoveItem(string productId, int quantity)
    {
        var cart = await cartService.RemoveItem(productId, quantity);
        return View(cart);
    }
}
