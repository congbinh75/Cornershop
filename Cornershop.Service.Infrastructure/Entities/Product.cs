using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities;

[Index(nameof(Id))]
public class Product : BaseEntity
{
    [Required]
    [MaxLength(250)]
    public required string Name { get; set; }

    [Required]
    public required string Code { get; set; }

    [Required]
    [MaxLength(15000)]
    public required string Description { get; set; }

    [Required]
    public required Subcategory Subcategory { get; set; }

    [Required]
    public required string SubcategoryId { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required decimal Price { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required decimal OriginalPrice { get; set; }

    [Required]
    [Range(0, 5)]
    public required decimal Rating { get; set; }

    [Required]
    public ICollection<ProductImage> ProductImages { get; set; } = [];

    [Required]
    public ICollection<Review> Reviews { get; set; } = [];

    [Required]
    public required Author Author { get; set; }

    [Required]
    public required Publisher Publisher { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required int Width { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required int Length { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required int Height { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required int Pages { get; set; }

    [Required]
    [Range(0, 2)]
    public required int Format { get; set; }

    [Required]
    [Range(0, 1000000000)]
    public required int Stock { get; set; }

    [Required]
    [Range(1900, 3000)]
    public required int PublishedYear { get; set; }

    [Required]
    public required bool IsVisible { get; set; } = false;

    public ICollection<CartDetail> CartDetails { get; set; } = [];

    public ICollection<OrderDetail> OrderDetails { get; set; } = [];
}
