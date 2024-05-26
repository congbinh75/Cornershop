using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;

namespace Cornershop.Service.Application.Controllers;

[Route("api/subcategory")]
[ApiController]
public class SubcategoryController(ISubCategoryService subcategoryService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var subcategory = await subcategoryService.GetById(id);
        return Ok(new GetSubcategoryResponse { Subcategory = subcategory });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize, string? categoryId)
    {
        ICollection<SubcategoryDTO> subcategories;
        if (string.IsNullOrEmpty(categoryId))
        {
            subcategories = await subcategoryService.GetAll(page, pageSize);
        }
        else
        {
            subcategories = await subcategoryService.GetAllByCategory(page, pageSize, categoryId);
        }

        var count = await subcategoryService.GetCount();
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllSubcategoryResponse { Subcategories = subcategories, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddSubcategoryRequest request)
    {
        var subcategory = await subcategoryService.Add(new SubcategoryDTO
        {
            Name = request.Name,
            Description = request.Description,
            Category = new CategoryDTO
            {
                Id = request.CategoryId
            }
        });
        return Ok(new AddSubcategoryResponse { Subcategory = subcategory });
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateSubcategoryRequest request)
    {
        var subcategory = await subcategoryService.Update(new SubcategoryDTO
        {
            Id = request.Id,
            Name = request.Name,
            Category = new CategoryDTO { Id = request.CategoryId },
            Description = request.Description
        });
        return Ok(new UpdateSubcategoryResponse { Subcategory = subcategory });
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Remove(string id)
    {
        await subcategoryService.Remove(id);
        return Ok(new RemoveCategoryResponse());
    }
}
