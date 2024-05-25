using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class CartService : ICartService
{
    public Task<CartDTO?> AddItem(string userId, string productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task<CartDTO?> GetByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAll(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveItem(string userId, string productId)
    {
        throw new NotImplementedException();
    }
}
