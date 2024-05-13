using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Cornershop.Service.Common.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Service.Infrastructure.UnitOfWorks;
using static Cornershop.Service.Common.Enums;

namespace Cornershop.Service.Domain.Services
{
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<UserDTO?> GetById(string id)
        {
            var user = await unitOfWork.GetRepository<User>().GetById(id);
            return Mapper.Map(user);
        }

        public async Task<UserDTO?> GetByCredentials(string email, string password)
        {
            var user = await unitOfWork.GetRepository<User>().Get(u => u.Email == email);
            if (user != null && !string.IsNullOrEmpty(password) && HashPassword(password, user.Salt).hashedPassword.Equals(user.Password))
            {
                return Mapper.Map(user);
            }
            return null;
        }

        public async Task<UserDTO?> Add(UserDTO userDTO)
        {
            (string hashed, byte[] salt) passAndSalt = HashPassword(userDTO.PlainPassword);
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = passAndSalt.hashed,
                IsEmailConfirmed = false,
                Salt = passAndSalt.salt,
                Role = userDTO.Role ?? (int)Role.Customer,
                IsBanned = false
            };
            await unitOfWork.GetRepository<User>().Add(user);
            await unitOfWork.SaveChanges();
            return Mapper.Map(user);
        }

        public Task<UserDTO?> Update(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> UpdatePassword(string id, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailConfirmation(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmEmail(string id, string token)
        {
            throw new NotImplementedException();
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
