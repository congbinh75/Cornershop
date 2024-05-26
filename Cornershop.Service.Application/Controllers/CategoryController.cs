using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;

namespace Cornershop.Service.Application.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var category = await categoryService.GetById(id);
        return Ok(new GetCategoryResponse { Category = category });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
    {
        var categories = await categoryService.GetAll(page, pageSize);
        var count = await categoryService.GetCount();
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllCategoryResponse { Categories = categories, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
    {
        var category = await categoryService.Add(new CategoryDTO
        {
            Name = request.Name,
            Description = request.Description
        });
        return Ok(new AddCategoryResponse { Category = category });
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
    {
        var category = await categoryService.Update(new CategoryDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        });
        return Ok(new UpdateCategoryResponse { Category = category });
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Remove(string id)
    {
        await categoryService.Remove(id);
        return Ok(new RemoveCategoryResponse());
    }
}
