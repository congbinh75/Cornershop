using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cornershop.Presentation.Customer.Pages;

public class LoginModel(IUserService userService) : PageModel
{
    [BindProperty]
    public new UserDTO? User { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (User == null)
        {
            ModelState.AddModelError(string.Empty, "User data is missing.");
            return Page();
        } 

        var token = await userService.Login(User);
        if (token != null)
        {
            Response.Cookies.Append("AuthCookie", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.UtcDateTime.AddDays(7)
            });

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("InvalidCredentials", "Email or password is incorrect.");
            return Page();
        }
    }
}