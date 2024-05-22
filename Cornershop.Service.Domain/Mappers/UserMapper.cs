using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class UserMapper
{
    public static UserDTO Map(this User user) =>
    new()
    {
        Id = user.Id,
        Username = user.Username,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        IsBanned = user.IsBanned,
        Role = user.Role,
        CreatedOn = user.CreatedOn,
        CreatedBy = user.CreatedBy != null ? Map(user.CreatedBy) : null,
        UpdatedOn = user.UpdatedOn,
        UpdatedBy = user.UpdatedBy != null ? Map(user.UpdatedBy) : null,
    };

    public static User Map(this UserDTO userDTO) =>
    new()
    {
        Id = userDTO.Id,
        Username = userDTO.Username,
        FirstName = userDTO.FirstName,
        LastName = userDTO.LastName,
        Email = userDTO.Email,
        IsBanned = userDTO.IsBanned,
        Role = userDTO.Role,
        IsEmailConfirmed = userDTO.IsEmailConfirmed,
        Password = "",
        Salt = [],
        Reviews = userDTO.Reviews.Select(ReviewMapper.Map).ToList(),
        Cart = userDTO.Cart.Map(),
        Orders = userDTO.Orders.Select(OrderMapper.Map).ToList(),
        CreatedOn = userDTO.CreatedOn,
        CreatedBy = Map(userDTO.CreatedBy),
        UpdatedOn = userDTO.UpdatedOn,
        UpdatedBy = Map(userDTO.UpdatedBy)
    };
}