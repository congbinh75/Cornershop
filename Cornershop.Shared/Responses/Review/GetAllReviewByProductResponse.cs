using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllReviewByProductResponse : BaseResponse
    {
        public ICollection<ReviewDTO> Reviews { get; set; }

        public int PagesCount { get; set; }
    }
}