namespace Cornershop.Shared.DTOs
{
    public class ProductImageDTO : BaseDTO
    {
        public ProductDTO Product { get; set; }

        public string ProductId { get; set; }

        public string ImageUrl { get; set; }

        public bool IsMainImage { get; set; }
    }
}