using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<Result<UserDTO?, string?>> GetById(string id);
        public Task<(ICollection<UserDTO> users, int count)> GetAll(int page, int pageSize, bool IsCustomerOnly = false);
        public Task<Result<UserDTO?, string?>> GetByCredentials(string email, string password);
        public Task<Result<UserDTO?, string?>> Add(UserDTO userDTO);
        public Task<Result<UserDTO?, string?>> Update(UserDTO userDTO);
        public Task<Result<bool, string?>> UpdatePassword(string id, string oldPassword, string newPassword);
        public Task<Result<bool, string?>> Remove(string id);
    }
}