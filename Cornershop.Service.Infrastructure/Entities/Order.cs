using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Order : BaseEntity
    {
        [Required]
        public required User User { get; set; }

        [Required]
        public required string Code { get; set; }

        [Required]
        public required ICollection<OrderDetail> OrderDetails { get; set;}

        [Required]
        public required decimal TotalPrice { get; set; }

        [Required]
        public required ICollection<Transaction> Transactions { get; set; }
    }
}