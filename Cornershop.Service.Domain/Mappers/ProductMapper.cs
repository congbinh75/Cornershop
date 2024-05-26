using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class ProductMapper
{
    public static ProductDTO Map(this Product product)
    {
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
            Subcategory = product.Subcategory.Map(),
            PublishedYear = product.PublishedYear,
            ProductImages = product.ProductImages.Select(x => x.Map()).ToList(),
            Rating = product.Rating,
            Reviews = product.Reviews.Select(x => x.Map()).ToList(),
            Author = product.Author.Map(),
            Publisher = product.Publisher.Map(),
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
            ProductImages = productDTO.ProductImages.Select(x => x.Map()).ToList(),
            Rating = productDTO.Rating,
            Reviews = productDTO.Reviews.Select(x => x.Map()).ToList(),
            Author = productDTO.Author.Map(),
            Publisher = productDTO.Publisher.Map(),
            IsVisible = productDTO.IsVisible,
            CreatedOn = productDTO.CreatedOn,
            CreatedBy = productDTO.CreatedBy.Map(),
            UpdatedOn = productDTO.UpdatedOn,
            UpdatedBy = productDTO.UpdatedBy.Map()
        };
    }
}