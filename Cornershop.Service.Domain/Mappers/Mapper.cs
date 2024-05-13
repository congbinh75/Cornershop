using Cornershop.Service.Common.DTOs;
using Cornershop.Service.Infrastructure.Entities;

namespace Cornershop.Service.Domain
{
    public static class Mapper 
    {
        public static UserDTO Map(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsBanned = user.IsBanned,
                Role = user.Role
            };
        }
    }
}