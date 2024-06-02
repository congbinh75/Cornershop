using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;

namespace Cornershop.Presentation.Customer.Services;

public class CartService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : ICartService
{
    public async Task<CartDTO?> GetCartByCurrentUser()
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/cart");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GetCartByCurrentUserResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null && response.Cart != null)
                {
                    return response.Cart;
                }
            }
        }
        return null;
    }

    public async Task<CartDTO?> AddItem(string productId, int quantity = 1)
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var addItemRequest = new AddItemCartRequest()
            {
                ProductId = productId,
                Quantity = quantity
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(addItemRequest), Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(httpClient.BaseAddress + "api/cart/add", jsonContent);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<AddItemCartResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null && response.Cart != null)
                {
                    return response.Cart;
                }
            }
        }
        return null;
    }

    public async Task<bool> RemoveItem(string productId, int quantity = 1)
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var addItemRequest = new RemoveItemCartRequest()
            {
                ProductId = productId,
                Quantity = quantity
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(addItemRequest), Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(httpClient.BaseAddress + "api/cart/remove", jsonContent);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<AddItemCartResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}