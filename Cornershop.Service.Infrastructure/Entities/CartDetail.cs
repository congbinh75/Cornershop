using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class CartDetail
    {
        [Key]
        [Required]
        public required Cart Cart { get; set; }

        [Key]
        [Required]
        public required Product Product { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required DateTimeOffset AddedOn { get; set; }
    }
}