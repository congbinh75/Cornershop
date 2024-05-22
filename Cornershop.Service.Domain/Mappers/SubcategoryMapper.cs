using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class SubcategoryMapper
{
    public static SubcategoryDTO Map(this Subcategory subcategory)
    {
        return new SubcategoryDTO
        {
            Id = subcategory.Id,
            Name = subcategory.Name,
            Description = subcategory.Description,
            Category = subcategory.Category?.Map(),
            CreatedOn = subcategory.CreatedOn,
            CreatedBy = subcategory.CreatedBy?.Map(),
            UpdatedOn = subcategory.UpdatedOn,
            UpdatedBy = subcategory.UpdatedBy?.Map()
        };
    }

    public static Subcategory Map(this SubcategoryDTO subcategoryDTO)
    {
        return new Subcategory
        {
            Id = subcategoryDTO.Id,
            Name = subcategoryDTO.Name,
            Description = subcategoryDTO.Description,
            Category = subcategoryDTO.Category.Map(),
            CreatedOn = subcategoryDTO.CreatedOn,
            CreatedBy = subcategoryDTO.CreatedBy.Map(),
            UpdatedOn = subcategoryDTO.UpdatedOn,
            UpdatedBy = subcategoryDTO.UpdatedBy.Map()
        };
    }
}