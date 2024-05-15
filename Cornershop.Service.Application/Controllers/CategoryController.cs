using Cornershop.Service.Common.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(category);
        }

        [HttpPut]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
        {
            var category = await categoryService.Add(new CategoryDTO{
                Name = request.Name,
                Description = request.Description
            });
            return Ok(category);
        }
    }
}