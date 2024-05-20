using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using static Cornershop.Service.Common.Enums;

namespace Cornershop.Service.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IConfiguration configuration, IStringLocalizer<SharedResources> stringLocalizer, IUserService userService) : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await userService.GetById(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = Shared.Constants.Page, int pageSize = Shared.Constants.PageSize)
        {
            var users = await userService.GetAll(page, pageSize);
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var userDTO = new UserDTO
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PlainPassword = request.Password,
                Role = (int)Role.Customer
            };
            var result = await userService.Add(userDTO);

            if (result.Success)
            {
                return Ok(new RegisterUserResponse { User = result.Value });
            }
            else
            {
                if (result.Error == Constants.ERR_EMAIL_ALREADY_REGISTERED)
                {
                    return Ok(new RegisterUserResponse { 
                        Status = Shared.Constants.Failure, 
                        Message = stringLocalizer[Constants.ERR_EMAIL_ALREADY_REGISTERED] 
                    });
                }
                else
                {
                    return Ok(new RegisterUserResponse { 
                        Status = Shared.Constants.Failure, 
                        Message = stringLocalizer[Constants.ERR_USERNAME_ALREADY_REGISTERED] 
                    });
                }
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var user = await userService.GetByCredentials(request.Email, request.Password);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"] ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, "User")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTimeOffset.UtcNow.UtcDateTime.AddDays(7),
                    signingCredentials: signIn);

                return Ok(new LoginUserResponse { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            else
            {
                return BadRequest(new LoginUserResponse
                {
                    Status = Shared.Constants.Failure,
                    Message = stringLocalizer[Constants.ERR_INVALID_CREDENTIALS].Value
                });
            }
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var userDTO = new UserDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await userService.Update(userDTO);
            return Ok(new UpdateUserResponse { User = result });
        }

        [HttpPost]
        [Route("recover-password")]
        public Task<IActionResult> SendResetPasswordEmail([FromBody] SendResetPasswordEmailRequestUser request)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Authorize]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            await userService.UpdatePassword(request.Id, request.OldPassword, request.NewPassword);
            return Ok(new UpdatePasswordUserResponse());
        }
    }
}