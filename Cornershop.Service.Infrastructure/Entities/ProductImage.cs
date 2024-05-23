using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities;

public class ProductImage : BaseEntity
{
    [Required]
    public required Product Product { get; set; }

    [Required]
    public required string ImageUrl { get; set; }

    [Required]
    public required bool IsMainImage { get; set; } = false;
}