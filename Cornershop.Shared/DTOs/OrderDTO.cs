using System.Collections.Generic;

namespace Cornershop.Shared.DTOs
{
    public class OrderDTO : BaseDTO
    {
        public UserDTO User { get; set; }

        public string Code { get; set; }

        public ICollection<OrderDetailDTO> OrderDetails { get; set;}

        public decimal TotalPrice { get; set; }

        public ICollection<TransactionDTO> Transactions { get; set; }
    }
}