using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface ICartService
    {
        public Task<CartDTO?> GetByUserId(string userId);
        public Task<CartDTO?> AddItem(string userId, string productId, int quantity);
        public Task<bool> RemoveItem(string userId, string productId);
        public Task<bool> RemoveAll(string userId);
    }
}