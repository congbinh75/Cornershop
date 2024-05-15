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
    [Route("api/user")]
    [ApiController]
    public class UserController(IConfiguration configuration, IStringLocalizer<SharedResources> stringLocalizer, IUserService userService) : ControllerBase
    {
        [HttpPut]
        [AllowAnonymous]
        [Route("register")]
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
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
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
                return BadRequest(stringLocalizer[Constants.ERR_INVALID_CREDENTIALS].Value);
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var userDTO = new UserDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await userService.Update(userDTO);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
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
            var result = await userService.UpdatePassword(request.Id, request.OldPassword, request.NewPassword);
            return Ok(result);
        }
    }
}