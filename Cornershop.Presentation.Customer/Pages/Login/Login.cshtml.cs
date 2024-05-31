using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cornershop.Presentation.Customer.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public UserDTO? User { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }


        return RedirectToAction("Index", "Home");
    }
}