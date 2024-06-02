using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class AddReviewResponse : BaseResponse
    {
        public ReviewDTO Review { get; set; }
    }
}