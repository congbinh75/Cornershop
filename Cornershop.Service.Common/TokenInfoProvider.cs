using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Cornershop.Service.Common
{
    public interface ITokenInfoProvider
    {
        string? Id { get; }
        string? Email { get; }
        string? Role { get; }
    }

    public class TokenInfoProvider(IHttpContextAccessor httpContextAccessor) : ITokenInfoProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? Id => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        public string? Email => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        public string? Role => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }
}
