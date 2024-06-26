using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;

namespace Cornershop.Presentation.Customer.Services;

public class UserService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : IUserService
{
    public async Task<UserDTO?> GetById(string id)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/user/" + id);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetCurrentUserResponse>(data, new JsonSerializerOptions
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

    public async Task<UserDTO?> GetCurrentUser()
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookieClient"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/user/current");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GetCurrentUserResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null && response.User != null)
                {
                    return response.User;
                }
            }
        }
        return null;
    }

    public async Task<string?> Login(UserDTO userDTO)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var loginRequest = new LoginUserRequest() { Email = userDTO.Email, Password = userDTO.PlainPassword };
        var jsonContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
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
        var jsonContent = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");
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

    public async Task<UserDTO?> Update(UserDTO userDTO)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var registerRequest = new UpdateUserRequest()
        {
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");
        var httpResponseMessage = await httpClient.PatchAsync(httpClient.BaseAddress + "api/user", jsonContent);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<UpdateUserResponse>(data, new JsonSerializerOptions
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
}