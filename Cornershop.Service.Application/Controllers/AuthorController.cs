using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;
using Microsoft.Extensions.Localization;

namespace Cornershop.Service.Application.Controllers;

[Route("api/author")]
[ApiController]
public class AuthorController(IAuthorService authorService, IStringLocalizer<SharedResources> stringLocalizer) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await authorService.GetById(id);
        if (result.Success)
        {
            return Ok(new GetAuthorResponse { Author = result.Value });
        }
        else
        {
            return BadRequest(new GetAuthorResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
    {
        var (authors, count) = await authorService.GetAll(page, pageSize);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllAuthorResponse { Authors = authors, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddAuthorRequest request)
    {
        var result = await authorService.Add(new AuthorDTO
        {
            Name = request.Name,
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new AddAuthorResponse { Author = result.Value });
        }
        else
        {
            return BadRequest(new AddAuthorResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateAuthorRequest request)
    {
        var result = await authorService.Update(new AuthorDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new UpdateAuthorResponse { Author = result.Value });
        }
        else
        {
            return BadRequest(new UpdateAuthorResponse
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
        var result = await authorService.Remove(id);
        if (result.Success)
        {
            return Ok(new RemoveAuthorResponse());
        }
        else
        {
            return BadRequest(new RemoveAuthorResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }
}
