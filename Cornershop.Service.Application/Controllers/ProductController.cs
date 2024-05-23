using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;

namespace Cornershop.Service.Application.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await productService.GetById(id);
            return Ok(new GetProductResponse{ Product = result });
        }

        [HttpGet]
        [Route("admin/{id}")]
        [Authorize(Roles = Constants.AdminAndStaff)]
        public async Task<IActionResult> Get(string id, [FromQuery] bool isHiddenIncluded)
        {
            var result = await productService.GetById(id, isHiddenIncluded);
            return Ok(new GetProductResponse{ Product = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
        {
            var result = await productService.GetAll(page, pageSize);
            var count = await productService.GetCount();
            var pagesCount = (int)Math.Ceiling((double)count / pageSize);
            return Ok(new GetAllProductResponse{ Products = result, PagesCount = pagesCount });
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = Constants.AdminAndStaff)]
        public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize, bool isHiddenIncluded)
        {
            var result = await productService.GetAll(page, pageSize, isHiddenIncluded);
            var count = await productService.GetCount();
            var pagesCount = (int)Math.Ceiling((double)count / pageSize);
            return Ok(new GetAllProductResponse{ Products = result, PagesCount = pagesCount });
        }

        [HttpPut]
        [Authorize(Roles = Constants.AdminAndStaff)]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request)
        {
            var product = await productService.Add(new ProductDTO{
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
                AuthorsIds = request.AuthorsIds,
                PublisherId = request.PublisherId,
                UploadImagesFiles = request.UploadedImagesFiles,
                UploadedMainImageFile = request.UploadedMainImageFile,
            });
            return Ok(new AddProductResponse { Product = product });
        }

        [HttpPatch]
        [Authorize(Roles = Constants.AdminAndStaff)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var product = await productService.Update(new ProductDTO{
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
                AuthorsIds = request.AuthorsIds,
                PublisherId = request.PublisherId,
                UploadImagesFiles = request.UploadedImagesFiles,
                ProductImagesIds = request.ProductImagesIds
            });
            return Ok(new UpdateProductResponse { Product = product });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            await productService.Remove(id);
            return Ok(new RemoveCategoryResponse());
        }
    }
}