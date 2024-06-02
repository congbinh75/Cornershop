using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetCartByCurrentUserResponse : BaseResponse
    {
        public CartDTO Cart { get; set; }
    }
}