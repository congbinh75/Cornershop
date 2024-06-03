using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class CartDetailMapper
{
    public static CartDetailDTO? Map(this CartDetail cartDetail)
    {
        if (cartDetail == null) return null;
        return new CartDetailDTO
        {
            CartId = cartDetail.CartId,
            ProductId = cartDetail.ProductId,
            Product = cartDetail.Product.Map(),
            Quantity = cartDetail.Quantity,
            AddedOn = cartDetail.AddedOn
        };
    }

    // public static CartDetail Map(this CartDetailDTO cartDetailDTO)
    // {
    //     return new CartDetail
    //     {
    //         Cart = cartDetailDTO.Cart.Map(),
    //         CartId = cartDetailDTO.CartId,
    //         Product = cartDetailDTO.Product.Map(),
    //         ProductId = cartDetailDTO.ProductId,
    //         Quantity = cartDetailDTO.Quantity,
    //         AddedOn = cartDetailDTO.AddedOn
    //     };
    // }
}