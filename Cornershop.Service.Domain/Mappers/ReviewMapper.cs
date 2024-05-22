using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class ReviewMapper
{
    public static ReviewDTO Map(this Review review)
    {
        return new ReviewDTO
        {
            Id = review.Id,
            Product = review.Product.Map(),
            User = review.User.Map(),
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedOn = review.CreatedOn,
            CreatedBy = review.CreatedBy?.Map(),
            UpdatedOn = review.UpdatedOn,
            UpdatedBy = review.UpdatedBy?.Map()
        };
    }

    public static Review Map(this ReviewDTO reviewDTO)
    {
        return new Review
        {
            Id = reviewDTO.Id,
            Product = reviewDTO.Product.Map(),
            User = reviewDTO.User.Map(),
            Rating = reviewDTO.Rating,
            Comment = reviewDTO.Comment,
            CreatedOn = reviewDTO.CreatedOn,
            CreatedBy = reviewDTO.CreatedBy.Map(),
            UpdatedOn = reviewDTO.UpdatedOn,
            UpdatedBy = reviewDTO.UpdatedBy.Map()
        };
    }
}