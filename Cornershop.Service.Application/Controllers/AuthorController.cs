using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;

namespace Cornershop.Service.Application.Controllers;

[Route("api/author")]
[ApiController]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var author = await authorService.GetById(id);
        return Ok(new GetAuthorResponse { Author = author });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
    {
        var authors = await authorService.GetAll(page, pageSize);
        var count = await authorService.GetCount();
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllAuthorResponse { Authors = authors, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddAuthorRequest request)
    {
        var author = await authorService.Add(new AuthorDTO
        {
            Name = request.Name,
            Description = request.Description
        });
        return Ok(new AddAuthorResponse { Author = author });
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdateAuthorRequest request)
    {
        var author = await authorService.Update(new AuthorDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        });
        return Ok(new UpdateAuthorResponse { Author = author });
    }

    [HttpDelete]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Remove([FromBody] string id)
    {
        await authorService.Remove(id);
        return Ok(new RemoveAuthorResponse());
    }
}
