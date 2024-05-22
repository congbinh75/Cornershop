using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class AuthorMapper
{
    public static AuthorDTO Map(this Author author)
    {
        return new AuthorDTO
        {
            Id = author.Id,
            Name = author.Name,
            Description = author.Description,
            Products = author.Products?.Select(ProductMapper.Map).ToList(),
            CreatedOn = author.CreatedOn,
            CreatedBy = author.CreatedBy?.Map(),
            UpdatedOn = author.UpdatedOn,
            UpdatedBy = author.UpdatedBy?.Map()
        };
    }

    public static Author Map(this AuthorDTO authorDTO)
    {
        return new Author
        {
            Id = authorDTO.Id,
            Name = authorDTO.Name,
            Description = authorDTO.Description,
            Products = authorDTO.Products.Select(ProductMapper.Map).ToList(),
            CreatedBy = authorDTO.CreatedBy?.Map(),
            CreatedOn = authorDTO.CreatedOn,
            UpdatedBy = authorDTO.UpdatedBy?.Map(),
            UpdatedOn = authorDTO.UpdatedOn
        };
    }
}