namespace Cornershop.Shared.DTOs
{
    public class ReviewDTO : BaseDTO
    {
        public ProductDTO Product { get; set; }

        public string ProductId { get; set; }

        public UserDTO User { get; set; }

        public string UserId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}