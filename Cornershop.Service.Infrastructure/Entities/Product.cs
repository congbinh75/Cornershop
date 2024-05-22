using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
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
        public required Subcategory Subcategory { get; set; }

        [Required]
        public required string SubcategoryId { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required decimal OriginalPrice { get; set; }

        [Required]
        public required decimal Rating { get; set; }

        [Required]
        public ICollection<Review> Reviews { get; set; } = [];

        [Required]
        public ICollection<Author> Authors { get; set; } = [];

        [Required]
        public required Publisher Publisher { get; set; }

        [Required]
        public required int Width { get; set; }

        [Required]
        public required int Length { get; set; }

        [Required]
        public required int Height { get; set; }

        [Required]
        public required int Pages { get; set; }

        [Required]
        public required int Format { get; set; }

        [Required]
        public required int Stock { get; set; }

        [Required]
        public required int PublishedYear { get; set; }

        [Required]
        public required bool IsVisible { get; set; } = false;

        public ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}