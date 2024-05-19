using System;

namespace Cornershop.Shared.DTOs
{
    public class CartDetailDTO
    {
        public CartDTO Cart { get; set; }

        public ProductDTO Product { get; set; }

        public int Quantity { get; set; }

        public DateTimeOffset AddedOn { get; set; }
    }
}