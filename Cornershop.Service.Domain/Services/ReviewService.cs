using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class ReviewSerivce(IDbContextFactory<CornershopDbContext> dbContextFactory) : IReviewService
{
    public async Task<(ICollection<ReviewDTO> reviews, int count)> GetAllByProduct(int page, int pageSize, string productId)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var reviews = await dbContext.Reviews.Where(r => r.Product.Id == productId).Include(r => r.User).ToListAsync();
        var count = dbContext.Reviews.Where(r => r.Product.Id == productId).Count();
        return (reviews.ConvertAll(ReviewMapper.Map)!, count);
    }

    public async Task<Result<ReviewDTO?, string?>> GetByProductAndUser(string productId, string userId)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var review = await dbContext.Reviews.Where(r => r.Product.Id == productId && r.User.Id == userId).Include(r => r.User).FirstOrDefaultAsync();
        if (review == null) return Constants.ERR_REVIEW_NOT_FOUND;
        return review.Map();
    }

    public async Task<Result<ReviewDTO?, string?>> Add(ReviewDTO reviewDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingReview = await dbContext.Reviews.Where(r => r.Product.Id == reviewDTO.ProductId && r.User.Id == reviewDTO.UserId).FirstOrDefaultAsync();
        if (existingReview != null) return Constants.ERR_REVIEW_FOR_PRODUCT_BY_USER_EXISTED;

        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == reviewDTO.ProductId);
        if (product == null) return Constants.ERR_PRODUCT_NOT_FOUND;
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == reviewDTO.UserId); 
        if (user == null) return Constants.ERR_USER_NOT_FOUND;
        var review = new Review
        {
            Product = product,
            User = user,
            Rating = reviewDTO.Rating,
            Comment = reviewDTO.Comment
        };
        await dbContext.Reviews.AddAsync(review);
        await dbContext.SaveChangesAsync();

        var reviews = await dbContext.Reviews.Where(r => r.Product.Id == reviewDTO.ProductId).ToListAsync();
        product.Rating = (decimal)reviews.Sum(r => r.Rating) / (decimal)reviews.Count;
        await dbContext.SaveChangesAsync();

        return review.Map();
    }

    public async Task<Result<bool, string?>> Remove(ReviewDTO reviewDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var review = await dbContext.Reviews.Where(r => r.Product.Id == reviewDTO.Product.Id && r.User.Id == reviewDTO.User.Id).FirstOrDefaultAsync();
        if (review == null) return Constants.ERR_REVIEW_NOT_FOUND;

        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == reviewDTO.Product.Id);
        if (product == null) return Constants.ERR_PRODUCT_NOT_FOUND;

        dbContext.Reviews.Remove(review);
        await dbContext.SaveChangesAsync();

        var reviews = await dbContext.Reviews.Where(r => r.Product.Id == reviewDTO.ProductId).ToListAsync();
        product.Rating = reviews.Sum(r => r.Rating) / reviews.Count;
        await dbContext.SaveChangesAsync();

        return true;
    }
}
