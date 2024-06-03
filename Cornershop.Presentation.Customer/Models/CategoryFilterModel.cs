using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Models;

public class CategoryFilterModel
{
    public ICollection<CategoryDTO> Categories { get; set; } = [];

    public ICollection<SubcategoryDTO> Subcategories { get; set; } = [];
}