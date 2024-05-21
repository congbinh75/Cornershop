using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetCurrentUserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
    }
}
