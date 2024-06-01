using Cornershop.Presentation.Customer.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class ProfileButton(IUserService userService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await userService.GetCurrentUser();
        return View(user);
    }
}