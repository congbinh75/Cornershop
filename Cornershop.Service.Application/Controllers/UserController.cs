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

namespace Cornershop.Service.Application.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(IConfiguration configuration,
    IStringLocalizer<SharedResources> stringLocalizer,
    ITokenInfoProvider tokenInfoProvider,
    IUserService userService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var user = await userService.GetById(id);
        return Ok(user);
    }

    [HttpGet]
    [Route("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await userService.GetById(userId);
        if (result.Success)
        {
            return Ok(new GetCurrentUserResponse { User = result.Value });
        }
        else
        {
            return BadRequest(new GetCurrentUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR]
            });
        }
    }

    [HttpGet]
    [Route("admin/current")]
    [Authorize(Roles = Constants.AdminAndStaff)]
    public async Task<IActionResult> GetCurrentUserAdmin()
    {
        string? userId = tokenInfoProvider.Id;
        if (userId == null) return Unauthorized();
        var result = await userService.GetById(userId);
        if (result.Success)
        {
            return Ok(new GetCurrentUserResponse { User = result.Value });
        }
        else
        {
            return BadRequest(new GetCurrentUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = Shared.Constants.Page, int pageSize = Shared.Constants.PageSize)
    {
        var (users, count) = await userService.GetAll(page, pageSize);
        var pageCount = (int)Math.Ceiling((double)count / pageSize);
        return Ok(new GetAllUserResponse { Users = users, PagesCount = pageCount });
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
            return BadRequest(new RegisterUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPut]
    [Route("/admin")]
    public async Task<IActionResult> AddStaff([FromBody] AddStaffUserRequest request)
    {
        if (!Enum.IsDefined(typeof(Enums.Role), request.Role))
        {
            return BadRequest(new RegisterUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[Constants.ERR_INVALID_ROLE_VALUE].Value
            });
        }

        var userDTO = new UserDTO
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PlainPassword = request.Password,
            Role = request.Role
        };
        var result = await userService.Add(userDTO);

        if (result.Success)
        {
            return Ok(new RegisterUserResponse { User = result.Value });
        }
        else
        {
            return BadRequest(new RegisterUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await userService.GetByCredentials(request.Email, request.Password);

        if (result.Success)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.NameIdentifier, result.Value?.Id ?? ""),
                    new Claim(ClaimTypes.Email, result.Value?.Email ?? ""),
                    new Claim(ClaimTypes.Role, ((Role)result.Value?.Role).ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTimeOffset.UtcNow.UtcDateTime.AddDays(7),
                signingCredentials: signIn);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Response.Cookies.Append("AuthCookie", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.UtcDateTime.AddDays(7)
            });

            return Ok(new LoginUserResponse { Token = tokenString });
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
        if (result.Success)
        {
            return Ok(new UpdateUserResponse { User = result.Value });
        }
        else
        {
            return BadRequest(new UpdateUserResponse
            {
                Status = Shared.Constants.Failure,
                Message = stringLocalizer[result.Error ?? Constants.ERR_UNEXPECTED_ERROR].Value
            });
        }
        
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

    [HttpPost]
    [Authorize]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Append("AuthCookie", string.Empty, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.UtcDateTime.AddDays(-1)
        });
        return Ok();
    }
}
