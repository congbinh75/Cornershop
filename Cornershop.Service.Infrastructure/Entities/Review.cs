using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Review : BaseEntity
    {
        public required Product Product { get; set; }

        public required User User { get; set; }

        [Required]
        [Range(1, 5)]
        public required int Rating { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Comment { get; set; }
    }
}