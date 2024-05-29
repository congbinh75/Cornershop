using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;

namespace Cornershop.Service.Application.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await productService.GetById(id);
        return Ok(new GetProductResponse { Product = result });
    }

    [HttpGet]
    [Route("admin/{id}")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Get(string id, [FromQuery] bool isHiddenIncluded)
    {
        var result = await productService.GetById(id, isHiddenIncluded);
        return Ok(new GetProductResponse { Product = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize,
         string? categoryId = null, string? subcategoryId = null, bool? isOrderedByPriceAscending = null)
    {
        (ICollection<ProductDTO> products, int count) = await productService.GetAll(page, pageSize, false,
            categoryId, subcategoryId, isOrderedByPriceAscending);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllProductResponse { Products = products, PagesCount = pagesCount });
    }

    [HttpGet]
    [Route("admin")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize, bool isHiddenIncluded, 
         string? categoryId = null, string? subcategoryId = null, bool? isOrderedByPriceAscending = null)
    {
        (ICollection<ProductDTO> products, int count) = await productService.GetAll(page, pageSize, isHiddenIncluded,
            categoryId, subcategoryId, isOrderedByPriceAscending);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllProductResponse { Products = products, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddProductRequest request)
    {
        var productImageDTOs = new List<ProductImageDTO>();
        if (!string.IsNullOrEmpty(request.MainImageUrl))
        {
            productImageDTOs.Add(new ProductImageDTO
            {
                ImageUrl = request.MainImageUrl,
                IsMainImage = true
            });
        }

        if (request.OtherImagesUrls != null && request.OtherImagesUrls.Count != 0)
        {
            productImageDTOs.AddRange(request.OtherImagesUrls.Select(url => new ProductImageDTO
            {
                ImageUrl = url,
                IsMainImage = false
            }));
        }

        var product = await productService.Add(new ProductDTO
        {
            Name = request.Name,
            Description = request.Description,
            SubcategoryId = request.SubcategoryId,
            Price = request.Price,
            OriginalPrice = request.OriginalPrice,
            Width = request.Width,
            Length = request.Length,
            Height = request.Height,
            Pages = request.Pages,
            Stock = request.Stock,
            Format = request.Format,
            PublishedYear = request.PublishedYear,
            AuthorId = request.AuthorId,
            PublisherId = request.PublisherId,
            ProductImages = productImageDTOs,
            IsVisible = request.IsVisible,
        });
        return Ok(new AddProductResponse { Product = product });
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
    {
        var productImages = new List<ProductImageDTO>();
        if (!string.IsNullOrEmpty(request.NewMainImageUrl))
        {
            productImages.Add(new ProductImageDTO
            {
                ImageUrl = request.NewMainImageUrl,
                IsMainImage = true
            });
        }

        if (request.NewOtherImagesUrls != null && request.NewOtherImagesUrls.Count != 0)
        {
            productImages.AddRange(request.NewOtherImagesUrls.Select(url => new ProductImageDTO
            {
                ImageUrl = url,
                IsMainImage = false
            }));
        }

        var product = await productService.Update(new ProductDTO
        {
            Id = request.Id,
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            SubcategoryId = request.SubcategoryId,
            Price = request.Price,
            OriginalPrice = request.OriginalPrice,
            Width = request.Width,
            Length = request.Length,
            Height = request.Height,
            Pages = request.Pages,
            Stock = request.Stock,
            Format = request.Format,
            PublishedYear = request.PublishedYear,
            AuthorId = request.AuthorId,
            PublisherId = request.PublisherId,
            ProductImages = productImages,
            ProductImagesIds = request.ProductImagesIds,
            IsVisible = request.IsVisible,
        });
        return Ok(new UpdateProductResponse { Product = product });
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Remove(string id)
    {
        await productService.Remove(id);
        return Ok(new RemoveCategoryResponse());
    }
}
