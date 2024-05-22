using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class CartDetailMapper
{
    public static CartDetailDTO Map(CartDetail cartDetail)
    {
        return new CartDetailDTO
        {
            Cart = cartDetail.Cart.Map(),
            Product = cartDetail.Product.Map(),
            Quantity = cartDetail.Quantity,
            AddedOn = cartDetail.AddedOn
        };
    }

    public static CartDetail Map(CartDetailDTO cartDetailDTO)
    {
        return new CartDetail
        {
            Cart = cartDetailDTO.Cart.Map(),
            CartId = cartDetailDTO.Cart.Id,
            Product = cartDetailDTO.Product.Map(),
            ProductId = cartDetailDTO.Product.Id,
            Quantity = cartDetailDTO.Quantity,
            AddedOn = cartDetailDTO.AddedOn
        };
    }
}