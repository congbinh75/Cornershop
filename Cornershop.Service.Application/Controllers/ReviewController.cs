using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Cornershop.Service.Common;
using Microsoft.Extensions.Localization;

namespace Cornershop.Service.Application.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController(IReviewService reviewService, 
    IStringLocalizer<SharedResources> stringLocalizer,
    ITokenInfoProvider tokenInfoProvider) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, int pageSize, string productId)
    {
        var (reviews, count) = await reviewService.GetAllByProduct(page, pageSize, productId);
        var pagesCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllReviewByProductResponse { Reviews = reviews, PagesCount = pagesCount });
    }

    [HttpGet]
    [Authorize]
    [Route("current-user")]
    public async Task<IActionResult> GetReviewOfProductByCurrentUser([FromQuery] string productId)
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await reviewService.GetByProductAndUser(productId, userId);
        if (result.Success)
        {
            return Ok(new GetReviewOfProductByCurrentUserResponse { Review = result.Value });
        }
        else
        {
            return BadRequest(new GetReviewOfProductByCurrentUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] AddReviewRequest request)
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await reviewService.Add(new ReviewDTO
        {
            ProductId = request.ProductId,
            UserId = userId,
            Rating = request.Rating,
            Comment = request.Comment
        });
        if (result.Success)
        {
            return Ok(new AddReviewResponse { Review = result.Value });
        }
        else
        {
            return BadRequest(new AddReviewResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    
    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> Remove(string id)
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var reviewDTO = new ReviewDTO
        {
            UserId = userId,
            ProductId = id
        };
        var result = await reviewService.Remove(reviewDTO);
        if (result.Success)
        {
            return Ok(new RemoveReviewResponse());
        }
        else
        {
            return BadRequest(new RemoveReviewResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }
}
