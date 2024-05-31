using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;

namespace Cornershop.Presentation.Customer.Services;

public class UserService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IUserService
{
    public Task<UserDTO?> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> Login(UserDTO userDTO)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var loginRequest = new LoginUserRequest() { Email = userDTO.Email, Password = userDTO.PlainPassword };
        var jsonContent = new StringContent (JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
        var httpResponseMessage = await httpClient.PostAsync(httpClient.BaseAddress + "api/user/login", jsonContent);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<LoginUserResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Token != null)
            {
                return response.Token;
            }
        }
        return null;
    }

    public async Task<UserDTO?> Register(UserDTO userDTO)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var registerRequest = new RegisterUserRequest() 
        { 
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            Username = userDTO.Username,
            Password = userDTO.PlainPassword 
        };
        var jsonContent = new StringContent (JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");
        var httpResponseMessage = await httpClient.PutAsync(httpClient.BaseAddress + "api/user", jsonContent);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<RegisterUserResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.User != null)
            {
                return response.User;
            }
        }
        return null;
    }

    public Task<UserDTO?> Update(UserDTO? productDTO)
    {
        throw new NotImplementedException();
    }
}