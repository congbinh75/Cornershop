namespace Cornershop.Shared.DTOs
{
    public class OrderDetailDTO
    {
        public OrderDTO Order { get; set; }

        public ProductDTO Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}