using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class UserController(IUserService userService) : Controller
{
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserDTO userDTO)
    {
        if (ModelState.IsValid)
        {
            var token = await userService.Login(userDTO);

            Response.Cookies.Append("AuthCookie", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.UtcDateTime.AddDays(7)
            });
        }
        return View();
    }
}
