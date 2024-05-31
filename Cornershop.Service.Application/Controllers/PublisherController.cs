using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;
using Microsoft.Extensions.Localization;

namespace Cornershop.Service.Application.Controllers;

[Route("api/publisher")]
[ApiController]
public class PublisherController(IPublisherService publisherService, IStringLocalizer<SharedResources> stringLocalizer) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await publisherService.GetById(id);
        if (result.Success)
        {
            return Ok(new GetPublisherResponse { Publisher = result.Value });
        }
        else
        {
            return BadRequest(new GetPublisherResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize)
    {
        var (publishers, count) = await publisherService.GetAll(page, pageSize);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllPublisherResponse { Publishers = publishers, PagesCount = pagesCount });
    }

    [HttpPut]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Add([FromBody] AddPublisherRequest request)
    {
        var result = await publisherService.Add(new PublisherDTO
        {
            Name = request.Name,
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new AddPublisherResponse { Publisher = result.Value });
        }
        else
        {
            return BadRequest(new AddPublisherResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPatch]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> Update([FromBody] UpdatePublisherRequest request)
    {
        var result = await publisherService.Update(new PublisherDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        });
        if (result.Success)
        {
            return Ok(new UpdatePublisherResponse { Publisher = result.Value });
        }
        else
        {
            return BadRequest(new UpdatePublisherResponse
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
        var result = await publisherService.Remove(id);
        if (result.Success)
        {
            return Ok(new RemovePublisherResponse());
        }
        else
        {
            return BadRequest(new RemovePublisherResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }
}
