using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(32)]
        public required string Name { get; set; }

        [Required]
        public required string Code { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required Category Category { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public ICollection<string> ImagesUrls { get; set; } = [];

        [Required]
        public required decimal Rating { get; set; }

        [Required]
        public ICollection<RatingVote> RatingVotes { get; set; } = [];
    }
}