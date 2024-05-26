using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Transaction : BaseEntity
    {
        [Required]
        public required Order Order { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public required decimal Amount { get; set; }
    }
}