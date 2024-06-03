using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Service.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Cornershop.Service.Common;
using Cornershop.Service.Domain.Mappers;

namespace Cornershop.Service.Domain.Services;

public class UserService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IUserService
{
    public async Task<Result<UserDTO?, string?>> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return Constants.ERR_USER_NOT_FOUND;
        return user.Map();
    }

    public async Task<(ICollection<UserDTO> users, int count)> GetAll(int page, int pageSize, bool IsCustomerOnly = false)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        List<User> users = [];
        int count = 0;
        if (IsCustomerOnly)
        {
            users = await dbContext.Users.Where(u => u.Role == (int)Enums.Role.Customer).Skip((page - 1) * pageSize).Take(pageSize)
                .OrderByDescending(a => a.CreatedOn).ToListAsync();
            count = dbContext.Users.Where(u => u.Role == (int)Enums.Role.Customer).Count();
        }
        else
        {
            users = await dbContext.Users.Skip((page - 1) * pageSize).Take(pageSize)
                .OrderByDescending(a => a.CreatedOn).ToListAsync();
            count = dbContext.Users.Count();
        }
        return (users.ConvertAll(UserMapper.Map)!, count);
    }

    public async Task<Result<UserDTO?, string?>> GetByCredentials(string email, string password)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null && !string.IsNullOrEmpty(password) && HashPassword(password, user.Salt).hashedPassword.Equals(user.Password))
        {
            return user.Map();
        }
        return Constants.ERR_USER_NOT_FOUND;
    }

    public async Task<Result<UserDTO?, string?>> Add(UserDTO userDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existingEmailUser = await dbContext.Users.Where(e => e.Email == userDTO.Email).FirstOrDefaultAsync();
        if (existingEmailUser != null) return Constants.ERR_EMAIL_ALREADY_REGISTERED;
        existingEmailUser = await dbContext.Users.Where(e => e.Username == userDTO.Username).FirstOrDefaultAsync();
        if (existingEmailUser != null) return Constants.ERR_USERNAME_ALREADY_REGISTERED;

        (string hashed, byte[] salt) = HashPassword(userDTO.PlainPassword);

        var user = new User
        {
            Username = userDTO.Username,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            Password = hashed,
            IsEmailConfirmed = false,
            Salt = salt,
            Role = userDTO.Role,
            IsBanned = false
        };
        await dbContext.Users.AddAsync(user);

        var cart = new Cart
        {
            UserId = user.Id,
            User = user,
            CartDetails = []
        };
        user.Cart = cart;
        await dbContext.Carts.AddAsync(cart);

        await dbContext.SaveChangesAsync();
        return user.Map();
    }

    public async Task<Result<UserDTO?, string?>> Update(UserDTO userDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDTO.Id);
        if (user == null) return Constants.ERR_USER_NOT_FOUND;
        user.FirstName = userDTO.FirstName ?? user.FirstName;
        user.LastName = userDTO.LastName ?? user.LastName;
        user.Role = userDTO.Role;
        user.IsBanned = userDTO.IsBanned;
        await dbContext.SaveChangesAsync();
        return user.Map();
    }

    public async Task<Result<bool, string?>> UpdatePassword(string id, string oldPassword, string newPassword)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return Constants.ERR_USER_NOT_FOUND;
        if (!string.IsNullOrEmpty(oldPassword) && HashPassword(oldPassword, user.Salt).hashedPassword.Equals(user.Password))
        {
            (string hashed, byte[] salt) = HashPassword(newPassword);
            user.Password = hashed;
            user.Salt = salt;
            await dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Result<bool, string?>> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return Constants.ERR_USER_NOT_FOUND;
        dbContext.Users.Remove(user);
        return true;
    }

    private static (string hashedPassword, byte[] salt) HashPassword(string password, byte[]? salt = null)
    {
        salt ??= RandomNumberGenerator.GetBytes(128 / 8);
        string hassed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        return (hassed, salt);
    }
}

