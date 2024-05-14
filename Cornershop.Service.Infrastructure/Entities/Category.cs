using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(32)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; } = [];
    }
}