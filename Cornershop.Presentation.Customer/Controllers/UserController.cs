using Cornershop.Presentation.Customer.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class UserController : Controller
{
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost] 
    public async Task<IActionResult> SubmitLogin()
    {
        return View();
    }
}
