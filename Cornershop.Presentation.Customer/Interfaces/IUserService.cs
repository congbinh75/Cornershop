using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Interfaces;

public interface IUserService
{
    public Task<UserDTO?> GetById(string id);
    public Task<UserDTO?> GetCurrentUser();
    public Task<UserDTO?> Register(UserDTO userDTO);
    public Task<string?> Login(UserDTO userDTO);
    public Task<UserDTO?> Update(UserDTO userDTO);
}