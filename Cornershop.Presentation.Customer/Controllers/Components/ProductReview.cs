using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Customer.Components;

public class ProductReviewModel
{
    public ICollection<ReviewDTO> Reviews { get; set; } = [];
    public ReviewDTO? CurrentUserReview { get; set; }
    public int PagesCount { get; set; }
    public int CurrentPage { get; set; }
    public string ProductId { get; set; } = "";

    public int NewRating { get; set; }
    public string NewComment { get; set; } = "";
}

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