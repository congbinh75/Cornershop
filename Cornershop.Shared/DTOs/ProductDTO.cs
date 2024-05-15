using System.Collections.Generic;

namespace Cornershop.Shared.DTOs
{
    public class ProductDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public CategoryDTO Category { get; set; }

        public decimal? Price { get; set; }

        public ICollection<string> ImagesUrls { get; set; }

        public decimal? Rating { get; set; }

        public ICollection<RatingVoteDTO> RatingVotes { get; set; }
    }
}