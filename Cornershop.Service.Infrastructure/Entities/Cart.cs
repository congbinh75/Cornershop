using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class Cart
    {
        [Key]
        [Required]
        public required User User { get; set; }

        [Required]
        public required ICollection<CartDetail> CartDetails { get; set; }
    }
}