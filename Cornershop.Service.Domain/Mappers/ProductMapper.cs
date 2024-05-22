using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class ProductMapper
{
    public static ProductDTO Map(this Product product)
    {
        var reviewsDTOs = product.Reviews.Select(ReviewMapper.Map).ToList();
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Description = product.Description,
            Price = product.Price,
            OriginalPrice = product.OriginalPrice,
            Width = product.Width,
            Length = product.Length,
            Height = product.Height,
            Pages = product.Pages,
            Format = product.Format,
            Stock = product.Stock,
            PublishedYear = product.PublishedYear,
            ImagesUrls = product.ImagesUrls,
            Rating = product.Rating,
            Reviews = product.Reviews.Select(ReviewMapper.Map).ToList(),
            CreatedOn = product.CreatedOn,
            CreatedBy = product.CreatedBy?.Map(),
            UpdatedOn = product.UpdatedOn,
            UpdatedBy = product.UpdatedBy?.Map()
        };
    }

    public static Product Map(this ProductDTO productDTO)
    {
        var ratingVoteDTOs = productDTO.Reviews.Select(ReviewMapper.Map).ToList();
        return new Product
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Code = productDTO.Code,
            Description = productDTO.Description,
            Subcategory = productDTO.Subcategory.Map(),
            SubcategoryId = productDTO.Subcategory.Id,
            Price = productDTO.Price,
            OriginalPrice = productDTO.OriginalPrice,
            Width = productDTO.Width,
            Length = productDTO.Length,
            Height = productDTO.Height,
            Pages = productDTO.Pages,
            Format = productDTO.Format,
            Stock = productDTO.Stock,
            PublishedYear = productDTO.PublishedYear,
            ImagesUrls = productDTO.ImagesUrls,
            Rating = productDTO.Rating,
            Reviews = productDTO.Reviews.Select(ReviewMapper.Map).ToList(),
            Authors = productDTO.Authors.Select(AuthorMapper.Map).ToList(),
            Publisher = productDTO.Publisher.Map(),
            IsVisible = productDTO.IsVisible,
            CreatedOn = productDTO.CreatedOn,
            CreatedBy = productDTO.CreatedBy.Map(),
            UpdatedOn = productDTO.UpdatedOn,
            UpdatedBy = productDTO.UpdatedBy.Map()
        };
    }
}