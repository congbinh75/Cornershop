using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class UpdateUserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
    }
}
