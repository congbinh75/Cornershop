using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetUserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
    }
}
