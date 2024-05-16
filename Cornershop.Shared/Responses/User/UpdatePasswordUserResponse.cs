using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class UpdatePasswordUserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
    }
}
