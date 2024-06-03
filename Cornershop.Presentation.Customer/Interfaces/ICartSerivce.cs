using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Interfaces
{
    public interface ICartService
    {
        public Task<CartDTO?> GetCartByCurrentUser();
        public Task<CartDTO?> AddItem(string productId, int quantity);
        public Task<bool> RemoveItem(string productId, int quantity);
    }
}