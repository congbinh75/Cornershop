using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class CartMapper
{
    public static CartDTO? Map(this Cart cart)
    {
        if (cart == null) return null;
        return new CartDTO
        {
            //User = cart.User.Map(),
            CartDetails = cart.CartDetails.Select(x => x.Map()).ToList()
        };
    }

    // public static Cart Map(this CartDTO cartDTO)
    // {
    //     return new Cart
    //     {
    //         User = cartDTO.User.Map(),
    //         UserId = cartDTO.User.Id,
    //         CartDetails = cartDTO.CartDetails.Select(x => x.Map()).ToList()
    //     };
    // }
}