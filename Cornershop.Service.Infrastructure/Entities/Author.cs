using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Author : BaseEntity
    {
        [Required]
        [MaxLength(128)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(1024)]
        public required string Description { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; } = [];
    }
}