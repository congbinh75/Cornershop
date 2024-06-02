using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cornershop.Presentation.Customer.Pages;

public class RegisterModel(IUserService userService) : PageModel
{
    [BindProperty]
    public new UserDTO? User { get; set; }

    [BindProperty]
    public string ConfirmPassword { get; set; } = "";

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (User == null)
        {
            ModelState.AddModelError(string.Empty, "User data is missing.");
            return Page();
        }

        if (User.PlainPassword != ConfirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Password and confirmation is not matched.");
            return Page();
        }

        var user = await userService.Register(User);
        if (user != null)
        {
            return RedirectToPage("Login");
        }

        ModelState.AddModelError(string.Empty, "Registration failed. Please try again later.");    
        return Page();
    }
}