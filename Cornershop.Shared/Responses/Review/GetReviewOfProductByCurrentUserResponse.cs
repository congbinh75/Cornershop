using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetReviewOfProductByCurrentUserResponse : BaseResponse
    {
        public ReviewDTO Review { get; set; }
    }
}