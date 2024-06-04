using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class CartService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ICartService
{
    public async Task<Result<CartDTO?, string?>> GetByUserId(string userId)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var cart = await dbContext.Carts.Where(c => c.User.Id == userId).Include(c => c.CartDetails)
            .ThenInclude(c => c.Product)
            .ThenInclude(p => p.ProductImages).FirstOrDefaultAsync();
        if (cart == null) return Constants.ERR_CART_NOT_FOUND;
        return cart.Map();
    }

    public async Task<Result<CartDTO?, string?>> AddItem(string userId, string productId, int quantity)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var cart = await dbContext.Carts.Where(c => c.User.Id == userId).Include(c => c.CartDetails).FirstOrDefaultAsync();
        if (cart == null) return Constants.ERR_CART_NOT_FOUND;
        var existingCartDetail = await dbContext.CartDetails.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Cart.User.Id == userId);
        if (existingCartDetail == null) return Constants.ERR_CART_DETAIL_NOT_FOUND;
        if (existingCartDetail == null) 
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return Constants.ERR_PRODUCT_NOT_FOUND;
            cart.CartDetails.Add(new CartDetail 
            {
                Cart = cart,
                CartId = userId,
                Quantity = quantity,
                Product = product,
                ProductId = product.Id,
                AddedOn = DateTime.UtcNow
            });
        }
        else
        {
            existingCartDetail.Quantity += quantity;
        }
        await dbContext.SaveChangesAsync();
        return cart.Map();
    }

    public async Task<Result<CartDTO?, string?>> RemoveItem(string userId, string productId, int quantity)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var cart = await dbContext.Carts.Where(c => c.User.Id == userId).FirstOrDefaultAsync();
        if (cart == null) return Constants.ERR_CART_NOT_FOUND;
        var existingCartDetail = await dbContext.CartDetails.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Cart.User.Id == userId);
        if (existingCartDetail == null) return Constants.ERR_CART_DETAIL_NOT_FOUND;
        if (existingCartDetail.Quantity > quantity)
        {
            existingCartDetail.Quantity -= quantity;
        }
        else
        {
            dbContext.CartDetails.Remove(existingCartDetail);
        }
        await dbContext.SaveChangesAsync();
        return cart.Map();
    }
}
