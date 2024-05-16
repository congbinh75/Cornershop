using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;

namespace Cornershop.Service.Application.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get([FromQuery] string id, bool isVisble = false)
        {
            var result = await productService.GetById(id);
            return Ok(new GetProductResponse{ Product = result });
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromBody] GetListProductRequest request)
        {
            var result = await productService.GetList(request.Page, request.PageSize);
            return Ok(new GetListProductResponse{ ProductList = result });
        }

        [HttpPut]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request)
        {
            var product = await productService.Add(new ProductDTO{
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                UploadImagesFiles = request.UploadedImagesFiles
            });
            return Ok(new AddProductResponse { Product = product });
        }

        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var product = await productService.Update(new ProductDTO{
                Id = request.Id,
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                UploadImagesFiles = request.UploadedImagesFiles
            });
            return Ok(new UpdateProductResponse { Product = product });
        }

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> Remove([FromBody] string id)
        {
            await productService.Remove(id);
            return Ok(new RemoveCategoryResponse());
        }
    }
}