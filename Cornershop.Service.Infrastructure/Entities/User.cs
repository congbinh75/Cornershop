using System.ComponentModel.DataAnnotations;
using Cornershop.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class User : BaseEntity
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

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

        public ICollection<Review> Reviews { get; set; } = [];

        public Cart? Cart { get; set; }

        public ICollection<Order> Orders { get; set; } = [];
    }
}
