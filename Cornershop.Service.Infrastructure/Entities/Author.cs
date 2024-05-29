using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Author : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; } = [];
    }
}