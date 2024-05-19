using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderDTO?> GetById(string id);
        public Task<ICollection<OrderDTO>> GetAllByUserId(string userId);
        public Task<OrderDTO?> Add(OrderDTO orderDTO);
        public Task<TransactionDTO?> Checkout(string userId);
        public Task<OrderDTO?> Update(OrderDTO orderDTO);
    }
}