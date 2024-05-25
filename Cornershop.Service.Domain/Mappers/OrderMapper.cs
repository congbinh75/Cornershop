using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class OrderMapper
{
    public static OrderDTO Map(this Order order)
    {
        return new OrderDTO
        {
            Id = order.Id,
            User = order.User.Map(),
            Code = order.Code,
            OrderDetails = order.OrderDetails.Select(x => x.Map()).ToList(),
            TotalPrice = order.TotalPrice,
            Transactions = order.Transactions.Select(x => x.Map()).ToList(),
            CreatedBy = order.CreatedBy?.Map(),
            CreatedOn = order.CreatedOn,
            UpdatedBy = order.UpdatedBy?.Map(),
            UpdatedOn = order.UpdatedOn,
        };
    }

    public static Order Map(this OrderDTO orderDTO)
    {
        return new Order
        {
            Id = orderDTO.Id,
            User = orderDTO.User.Map(),
            Code = orderDTO.Code,
            OrderDetails = orderDTO.OrderDetails.Select(x => x.Map()).ToList(),
            TotalPrice = orderDTO.TotalPrice,
            Transactions = orderDTO.Transactions.Select(x => x.Map()).ToList(),
            CreatedBy = orderDTO.CreatedBy?.Map(),
            CreatedOn = orderDTO.CreatedOn,
            UpdatedBy = orderDTO.UpdatedBy.Map(),
            UpdatedOn = orderDTO.UpdatedOn,
        };
    }
}