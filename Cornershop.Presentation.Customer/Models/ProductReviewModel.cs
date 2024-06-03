using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Models;

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