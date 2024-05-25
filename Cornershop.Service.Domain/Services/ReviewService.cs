using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class ReviewSerivce : IReviewService
{
    public Task<ICollection<ReviewDTO>> GetByProduct(int page, int pageSize, bool isVisible = false)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDTO?> Add(ReviewDTO productDTO)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Remove(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDTO?> Update(ReviewDTO productDTO)
    {
        throw new NotImplementedException();
    }
}
