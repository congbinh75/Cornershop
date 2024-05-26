using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;

namespace Cornershop.Service.Application.Controllers;

[Route("api/publisher")]
[ApiController]
public class PublisherController(IPublisherService publisherService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var publisher = await publisherService.GetById(id);
        return Ok(new GetPublisherResponse { Publisher = publisher });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
    {
        var authors = await publisherService.GetAll(page, pageSize);
        var count = await publisherService.GetCount();
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllPublisherResponse { Publishers = authors, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddPublisherRequest request)
    {
        var publisher = await publisherService.Add(new PublisherDTO
        {
            Name = request.Name,
            Description = request.Description
        });
        return Ok(new AddPublisherResponse { Publisher = publisher });
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdatePublisherRequest request)
    {
        var publisher = await publisherService.Update(new PublisherDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        });
        return Ok(new UpdatePublisherResponse { Publisher = publisher });
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Remove(string id)
    {
        await publisherService.Remove(id);
        return Ok(new RemovePublisherResponse());
    }
}
