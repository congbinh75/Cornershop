using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class OrderDetailMapper
{
    public static OrderDetailDTO? Map(this OrderDetail orderDetail)
    {
        if (orderDetail == null) return null;
        return new OrderDetailDTO
        {
            Order = orderDetail.Order.Map(),
            Product = orderDetail.Product.Map(),
            Quantity = orderDetail.Quantity,
            Price = orderDetail.Price
        };
    }

    // public static OrderDetail Map(this OrderDetailDTO orderDetailDTO)
    // {
    //     return new OrderDetail
    //     {
    //         Order = orderDetailDTO.Order.Map(),
    //         OrderId = orderDetailDTO.Order.Id,
    //         Product = orderDetailDTO.Product.Map(),
    //         ProductId = orderDetailDTO.Product.Id,
    //         Quantity = orderDetailDTO.Quantity,
    //         Price = orderDetailDTO.Price
    //     };
    // }
}