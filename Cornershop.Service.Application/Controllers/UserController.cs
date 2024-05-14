using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cornershop.Service.Common;
using Cornershop.Service.Common.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using static Cornershop.Service.Common.Enums;

namespace NashtechECommerceService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IConfiguration configuration, IStringLocalizer<SharedResources> stringLocalizer, IUserService userService) : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest input)
        {
            var userDTO = new UserDTO
            {
                Name = input.Name,
                Email = input.Email,
                PlainPassword = input.Password,
                Role = (int)Role.Customer
            };
            await userService.Add(userDTO);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest input)
        {
            var user = await userService.GetByCredentials(input.Email, input.Password);

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

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest(stringLocalizer[Constants.ERR_INVALID_CREDENTIALS].Value);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest input)
        {
            var userDTO = new UserDTO
            {
                Name = input.Name,
                Email = input.Email,
            };
            var result = await userService.Update(userDTO);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("recover-password")]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] SendResetPasswordEmailRequestUser input)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest input)
        {
            var result = await userService.UpdatePassword(input.Id, input.OldPassword, input.NewPassword);
            return Ok(result);
        }
    }
}