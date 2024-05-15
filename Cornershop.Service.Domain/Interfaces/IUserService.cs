using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<UserDTO?> GetById(string id);
        public Task<UserDTO?> GetByCredentials(string email, string password);
        public Task<UserDTO?> Add(UserDTO userDTO);
        public Task<UserDTO?> Update(UserDTO userDTO);
        public Task<bool> UpdatePassword(string id, string oldPassword, string newPassword);
        public Task<bool> Remove(string id);
        public Task<bool> SendEmailConfirmation(string id);
        public Task<bool> ConfirmEmail(string id, string token);
    }
}