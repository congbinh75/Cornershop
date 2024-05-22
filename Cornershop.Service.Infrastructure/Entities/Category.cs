using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(32)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public ICollection<Subcategory> Subcategories { get; set; } = [];

        [Required]
        public ICollection<Product> Products { get; set; } = [];
    }
}