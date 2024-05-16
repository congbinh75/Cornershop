using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;

namespace Cornershop.Service.Application.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var category = await categoryService.GetById(id);
            return Ok(new GetCategoryResponse{ Category = category });
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var categoryList = await categoryService.GetAll();
            return Ok(new GetAllCategoryResponse{ CategoryList = categoryList });
        }

        [HttpPut]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
        {
            var category = await categoryService.Add(new CategoryDTO{
                Name = request.Name,
                Description = request.Description
            });
            return Ok(new AddCategoryResponse{ Category = category });
        }

        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
        {
            var category = await categoryService.Update(new CategoryDTO{ 
                Id = request.Id, 
                Name = request.Name, 
                Description = request.Description });
            return Ok(new UpdateCategoryResponse { Category = category });
        }

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> Remove([FromBody] string id)
        {
            await categoryService.Remove(id);
            return Ok(new RemoveCategoryResponse());
        }
    }
}