using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class OrderDetail
    {
        [Key]
        [Required]
        public required Order Order { get; set; }

        [Key]
        [Required]
        public required Product Product { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required decimal Price { get; set; }
    }
}