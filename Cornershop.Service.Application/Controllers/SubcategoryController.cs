using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;
using Microsoft.Extensions.Localization;

namespace Cornershop.Service.Application.Controllers;

[Route("api/subcategory")]
[ApiController]
public class SubcategoryController(ISubcategoryService subcategoryService, IStringLocalizer<SharedResources> stringLocalizer) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await subcategoryService.GetById(id);
        if (result.Success)
        {
            return Ok(new GetSubcategoryResponse { Subcategory = result.Value });
        }
        else
        {
            return BadRequest(new GetSubcategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize, string? categoryId)
    {
        var (subcategories, count) = await subcategoryService.GetAll(page, pageSize, categoryId);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllSubcategoryResponse { Subcategories = subcategories, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddSubcategoryRequest request)
    {
        var result = await subcategoryService.Add(new SubcategoryDTO
        {
            Name = request.Name,
            Description = request.Description,
            Category = new CategoryDTO
            {
                Id = request.CategoryId
            }
        });
        if (result.Success)
        {
            return Ok(new AddSubcategoryResponse { Subcategory = result.Value });
        }
        else
        {
            return BadRequest(new AddSubcategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateSubcategoryRequest request)
    {
        var result = await subcategoryService.Update(new SubcategoryDTO
        {
            Id = request.Id,
            Name = request.Name,
            Category = new CategoryDTO { Id = request.CategoryId },
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new UpdateSubcategoryResponse { Subcategory = result.Value });
        }
        else
        {
            return BadRequest(new UpdateSubcategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Remove(string id)
    {
        var result = await subcategoryService.Remove(id);
        if (result.Success)
        {
            return Ok(new RemoveSubcategoryResponse());
        }
        else
        {
            return BadRequest(new RemoveSubcategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }
}
