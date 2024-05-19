using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [PrimaryKey(nameof(CartId), nameof(ProductId))]
    public class CartDetail
    {
        [Required]
        public required Cart Cart { get; set; }

        [Required]
        public required string CartId { get; set; }

        [Required]
        public required Product Product { get; set; }

        [Required]
        public required string ProductId { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required DateTimeOffset AddedOn { get; set; }
    }
}