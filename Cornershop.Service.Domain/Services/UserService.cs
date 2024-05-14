using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Cornershop.Service.Common.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Entities;
using static Cornershop.Service.Common.Enums;
using Cornershop.Service.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class UserService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IUserService
    {
        public async Task<UserDTO?> GetById(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return Mapper.Map(user);
        }

        public async Task<UserDTO?> GetByCredentials(string email, string password)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && !string.IsNullOrEmpty(password) && HashPassword(password, user.Salt).hashedPassword.Equals(user.Password))
            {
                return Mapper.Map(user);
            }
            return null;
        }

        public async Task<UserDTO?> Add(UserDTO userDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            (string hashed, byte[] salt) = HashPassword(userDTO.PlainPassword);
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = hashed,
                IsEmailConfirmed = false,
                Salt = salt,
                Role = userDTO.Role ?? (int)Role.Customer,
                IsBanned = false
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return Mapper.Map(user);
        }

        public async Task<UserDTO?> Update(UserDTO userDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDTO.Id);
            if (existingUser == null) return null;
            existingUser.Name = userDTO.Name ?? existingUser.Name;
            existingUser.Email = userDTO.Email ?? existingUser.Email; 
            existingUser.Role = userDTO.Role ?? existingUser.Role;
            existingUser.IsBanned = userDTO.IsBanned ?? existingUser.IsBanned;
            await dbContext.SaveChangesAsync();
            return userDTO;
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
}
