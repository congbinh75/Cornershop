using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class AddItemCartResponse : BaseResponse
    {
        public CartDTO Cart { get; set; }
    }
}
