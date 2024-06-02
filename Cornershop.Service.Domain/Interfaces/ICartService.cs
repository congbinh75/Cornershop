using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface ICartService
    {
        public Task<Result<CartDTO?, string?>> GetByUserId(string userId);
        public Task<Result<CartDTO?, string?>> AddItem(string userId, string productId, int quantity);
        public Task<Result<CartDTO?, string?>> RemoveItem(string userId, string productId, int quantity);
    }
}