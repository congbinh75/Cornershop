using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;
using Microsoft.Extensions.Localization;

namespace Cornershop.Service.Application.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController(ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await categoryService.GetById(id);
        if (result.Success)
        {
            return Ok(new GetCategoryResponse { Category = result.Value });
        }
        else
        {
            return BadRequest(new GetCategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
    {
        var (categories, count) = await categoryService.GetAll(page, pageSize);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllCategoryResponse { Categories = categories, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
    {
        var result = await categoryService.Add(new CategoryDTO
        {
            Name = request.Name,
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new AddCategoryResponse { Category = result.Value });
        }
        else
        {
            return BadRequest(new AddCategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
    {
        var result = await categoryService.Update(new CategoryDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new UpdateCategoryResponse { Category = result.Value });
        }
        else
        {
            return BadRequest(new UpdateCategoryResponse
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
        var result = await categoryService.Remove(id);
        if (result.Success)
        {
            return Ok(new RemoveCategoryResponse());
        }
        else
        {
            return BadRequest(new RemoveCategoryResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }
}
