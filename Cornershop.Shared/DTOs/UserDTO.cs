using System.Collections.Generic;

namespace Cornershop.Shared.DTOs
{
    public class UserDTO :  BaseDTO
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public bool IsEmailConfirmed { get; set; }

        public string Password { get; set; }

        public string PlainPassword { get; set; }

        public bool IsBanned { get; set; }

        public int Role { get; set; }

        public CartDTO Cart { get; set; }

        public ICollection<OrderDTO> Orders { get; set; }

        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}
