using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class ProductImageMapper
{
    public static ProductImageDTO Map(this ProductImage productImage)
    {
        return new ProductImageDTO
        {
            Id = productImage.Id,
            ProductId = productImage.ProductId,
            ImageUrl = productImage.ImageUrl,
            IsMainImage = productImage.IsMainImage,
            CreatedOn = productImage.CreatedOn,
            CreatedBy = productImage.CreatedBy?.Map(),
            UpdatedOn = productImage.UpdatedOn,
            UpdatedBy = productImage.UpdatedBy?.Map()
        };
    }

    public static ProductImage Map(this ProductImageDTO productImageDTO)
    {
        return new ProductImage
        {
            Id = productImageDTO.Id,
            Product = productImageDTO.Product.Map(),
            ProductId = productImageDTO.Product.Id,
            ImageUrl = productImageDTO.ImageUrl,
            IsMainImage = productImageDTO.IsMainImage,
            CreatedOn = productImageDTO.CreatedOn,
            CreatedBy = productImageDTO.CreatedBy?.Map(),
            UpdatedOn = productImageDTO.UpdatedOn,
            UpdatedBy = productImageDTO.UpdatedBy?.Map()
        };
    }
}