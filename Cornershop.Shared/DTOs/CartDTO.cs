using System.Collections.Generic;

namespace Cornershop.Shared.DTOs
{
    public class CartDTO : BaseDTO
    {
        public UserDTO User { get; set; }

        public ICollection<CartDetailDTO> CartDetails { get; set; }
    }
}