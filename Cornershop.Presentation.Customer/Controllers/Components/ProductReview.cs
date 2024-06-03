using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Models;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class ProductReview(IReviewService reviewService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string productId, int page, int pageSize)
    {
        var (reviews, count) = await reviewService.GetAllByProduct(productId, page, pageSize);
        var currentUserReview = await reviewService.GetReviewOfProductByCurrentUser(productId);
        var model = new ProductReviewModel
        {
            Reviews = reviews,
            CurrentUserReview = currentUserReview,
            CurrentPage = page,
            PagesCount = count,
            ProductId = productId
        };
        return View(model);
    }
}