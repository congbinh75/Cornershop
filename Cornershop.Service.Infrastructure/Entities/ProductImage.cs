using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities;

[Index(nameof(Id))]
public class ProductImage : BaseEntity
{
    [Required]
    public required Product Product { get; set; }

    [Required]
    public required string ProductId { get; set; }

    [Required]
    public required string ImageUrl { get; set; }

    [Required]
    public required bool IsMainImage { get; set; } = false;
}