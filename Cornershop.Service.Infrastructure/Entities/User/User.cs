using System.ComponentModel.DataAnnotations;
using Cornershop.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class User : BaseEntity
    {
        [Required]
        [MinLength(6)]
        [MaxLength(32)]
        public required string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        public required bool IsEmailConfirmed { get; set; } = false;

        [Required]
        [Range(0, 2)]
        public required int Role { get; set; } = (int)Enums.Role.Customer;

        [Required]
        public required string Password { get; set; }

        [Required]
        public required byte[] Salt { get; set; } = [];

        [Required]
        public required bool IsBanned { get; set; } = false;

        public string? EmailConfirmationToken { get; set; }
    }
}
