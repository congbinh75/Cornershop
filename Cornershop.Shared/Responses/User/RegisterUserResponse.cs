﻿using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class RegisterUserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
    }
}
