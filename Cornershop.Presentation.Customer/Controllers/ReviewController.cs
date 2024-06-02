using Cornershop.Presentation.Customer.Components;
using Cornershop.Presentation.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cornershop.Presentation.Controllers;

public class ReviewController(IReviewService reviewService) : Controller
{
    public async Task<IActionResult> Submit(ProductReviewModel productReviewModel)
    {
        await reviewService.Add(productReviewModel.ProductId, productReviewModel.NewRating, productReviewModel.NewComment);
        return RedirectToAction("Detail", "Product", new { id = productReviewModel.ProductId });
    }
}
