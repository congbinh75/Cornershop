using Cornershop.Presentation.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class UserController(IUserService userService) : Controller
{
    public async Task<IActionResult> Detail(string id)
    {
        var user = await userService.GetById(id);
        return View(user);
    }

    public IActionResult Logout()
    {
        Response.Cookies.Append("AuthCookieClient", string.Empty, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.UtcDateTime.AddDays(-1)
        });
        return RedirectToAction("Index", "Home");
    }
}
