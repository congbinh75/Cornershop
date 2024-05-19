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
        public required decimal Amount { get; set; }
    }
}