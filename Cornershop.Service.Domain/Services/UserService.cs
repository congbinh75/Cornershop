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
    public async Task<UserDTO?> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception(); //TO BE FIXED
        return user.Map();
    }

    public async Task<ICollection<UserDTO>> GetAll(int page, int pageSize)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var users = await dbContext.Users.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedOn).ToListAsync() ?? throw new Exception(); //TO BE FIXED
        return users.ConvertAll(UserMapper.Map);
    }

    public async Task<int> GetCount()
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return dbContext.Users.Count();
    }

    public async Task<UserDTO?> GetByCredentials(string email, string password)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null && !string.IsNullOrEmpty(password) && HashPassword(password, user.Salt).hashedPassword.Equals(user.Password))
        {
            return user.Map();
        }
        return null;
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

    public async Task<UserDTO?> Update(UserDTO userDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDTO.Id) ?? throw new Exception(); //TO BE FIXED
        user.FirstName = userDTO.FirstName ?? user.FirstName;
        user.LastName = userDTO.LastName ?? user.LastName;
        user.Role = userDTO.Role;
        user.IsBanned = userDTO.IsBanned;
        await dbContext.SaveChangesAsync();
        return user.Map();
    }

    public async Task<bool> UpdatePassword(string id, string oldPassword, string newPassword)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception();
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

    public async Task<bool> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception();
        dbContext.Users.Remove(user);
        return true;
    }

    public Task<bool> SendEmailConfirmation(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ConfirmEmail(string id, string token)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception();
        if (user.EmailConfirmationToken == token)
        {
            user.IsEmailConfirmed = true;
            await dbContext.SaveChangesAsync();
            return true;
        }
        return false;
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

