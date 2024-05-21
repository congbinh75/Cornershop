using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<UserDTO?> GetById(string id);
        public Task<ICollection<UserDTO>> GetAll(int page, int pageSize);
        public Task<int> GetCount();
        public Task<UserDTO?> GetByCredentials(string email, string password);
        public Task<Result<UserDTO?, string?>> Add(UserDTO userDTO);
        public Task<UserDTO?> Update(UserDTO userDTO);
        public Task<bool> UpdatePassword(string id, string oldPassword, string newPassword);
        public Task<bool> Remove(string id);
        public Task<bool> SendEmailConfirmation(string id);
        public Task<bool> ConfirmEmail(string id, string token);
    }
}