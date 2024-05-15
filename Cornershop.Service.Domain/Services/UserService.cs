using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Cornershop.Shared.DTOs;
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
            var existingEmailUser = await dbContext.Users.FirstOrDefaultAsync(e => e.Email == userDTO.Email);
            if (existingEmailUser == null) return null;
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
            await dbContext.SaveChangesAsync();
            return Mapper.Map(user);
        }

        public async Task<UserDTO?> Update(UserDTO userDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDTO.Id) ?? throw new Exception(); //TO BE FIXED
            user.Username = userDTO.Username ?? user.Username;
            user.FirstName = userDTO.FirstName ?? user.FirstName;
            user.LastName = userDTO.LastName ?? user.LastName;
            user.Email = userDTO.Email ?? user.Email; 
            user.Role = userDTO.Role;
            user.IsBanned = userDTO.IsBanned;
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
