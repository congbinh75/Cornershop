using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;
using Microsoft.Extensions.Localization;

namespace Cornershop.Service.Application.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController(ICartService cartService,
IStringLocalizer<SharedResources> stringLocalizer,
ITokenInfoProvider tokenInfoProvider) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCartByCurrentUser()
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await cartService.GetByUserId(userId);
        if (result.Success)
        {
            return Ok(new GetCartByCurrentUserResponse { Cart = result.Value });
        }
        else
        {
            return BadRequest(new GetCartByCurrentUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR]
            });
        }
    }

    [HttpPost]
    [Authorize]
    [Route("add")]
    public async Task<IActionResult> AddItem(AddItemCartRequest request)
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await cartService.AddItem(userId, request.ProductId, request.Quantity);
        if (result.Success)
        {
            return Ok(new AddItemCartResponse { Cart = result.Value });
        }
        else
        {
            return BadRequest(new AddItemCartResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR]
            });
        }
    }

    [HttpPost]
    [Authorize]
    [Route("remove")]
    public async Task<IActionResult> RemoveItem(RemoveItemCartRequest request)
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await cartService.AddItem(userId, request.ProductId, request.Quantity);
        if (result.Success)
        {
            return Ok(new RemoveItemCartResponse { Cart = result.Value });
        }
        else
        {
            return BadRequest(new RemoveItemCartResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR]
            });
        }
    }
}