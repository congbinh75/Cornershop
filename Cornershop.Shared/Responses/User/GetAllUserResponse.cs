using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllUserResponse : BaseResponse
    {
        public ICollection<UserDTO> Users { get; set; }

        public int PagesCount { get; set; }
    }
}
