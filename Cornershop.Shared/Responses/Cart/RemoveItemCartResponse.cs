using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class RemoveItemCartResponse : BaseResponse
    {
        public CartDTO Cart { get; set; }
    }
}
