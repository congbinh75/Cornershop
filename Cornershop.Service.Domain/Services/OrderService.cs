using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class OrderService : IOrderService
    {
        public Task<OrderDTO?> Add(OrderDTO orderDTO)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionDTO?> Checkout(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderDTO>> GetAllByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO?> Update(OrderDTO orderDTO)
        {
            throw new NotImplementedException();
        }
    }
}