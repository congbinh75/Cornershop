using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class PublisherMapper
{
    public static PublisherDTO Map(this Publisher publisher)
    {
        return new PublisherDTO
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Description = publisher.Description,
            CreatedOn = publisher.CreatedOn,
            CreatedBy = publisher.CreatedBy?.Map(),
            UpdatedOn = publisher.UpdatedOn,
            UpdatedBy = publisher.UpdatedBy?.Map()
        };
    }

    public static Publisher Map(this PublisherDTO publisherDTO)
    {
        return new Publisher
        {
            Id = publisherDTO.Id,
            Name = publisherDTO.Name,
            Description = publisherDTO.Description,
            Products = publisherDTO.Products.Select(x => x.Map()).ToList(),
            CreatedBy = publisherDTO.CreatedBy.Map(),
            CreatedOn = publisherDTO.CreatedOn,
            UpdatedBy = publisherDTO.UpdatedBy.Map(),
            UpdatedOn = publisherDTO.UpdatedOn,
        };
    }
}