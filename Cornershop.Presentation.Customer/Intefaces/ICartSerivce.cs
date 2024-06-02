using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface ICartService
    {
        public Task<CartDTO?> GetCartByCurrentUser();
        public Task<CartDTO?> AddItem(string productId, int quantity);
        public Task<bool> RemoveItem(string productId, int quantity);
    }
}