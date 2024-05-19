using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [PrimaryKey(nameof(UserId))]
    public class Cart
    {
        [Key]
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required User User { get; set; }

        [Required]
        public required ICollection<CartDetail> CartDetails { get; set; }
    }
}