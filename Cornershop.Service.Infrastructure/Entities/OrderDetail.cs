using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderDetail
    {
        [Required]
        public required Order Order { get; set; }

        [Required]
        public required string OrderId { get; set; }

        [Required]
        public required Product Product { get; set; }

        [Required]
        public required string ProductId { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required decimal Price { get; set; }
    }
}