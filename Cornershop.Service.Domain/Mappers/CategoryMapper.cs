using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class CategoryMapper
{
    public static CategoryDTO Map(this Category category) =>
    new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        CreatedOn = category.CreatedOn,
        CreatedBy = category.CreatedBy?.Map(),
        UpdatedOn = category.UpdatedOn,
        UpdatedBy = category.UpdatedBy?.Map()
    };

    public static Category Map(this CategoryDTO categoryDTO) =>
    new()
    {
        Id = categoryDTO.Id,
        Name = categoryDTO.Name,
        Description = categoryDTO.Description,
        CreatedOn = categoryDTO.CreatedOn,
        CreatedBy = categoryDTO.CreatedBy.Map(),
        UpdatedOn = categoryDTO.UpdatedOn,
        UpdatedBy = categoryDTO.UpdatedBy.Map()
    };
}