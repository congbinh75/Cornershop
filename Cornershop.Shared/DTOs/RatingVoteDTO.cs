namespace Cornershop.Shared.DTOs
{
    public class RatingVoteDTO
    {
        public ProductDTO Product { get; set; }

        public UserDTO User { get; set; }

        public int Rate { get; set; }
    }
}