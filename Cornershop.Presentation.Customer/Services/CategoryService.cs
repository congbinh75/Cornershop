using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;

namespace Cornershop.Presentation.Customer.Services;

public class CategoryService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ICategoryService
{
    public async Task<CategoryDTO?> GetById(string id)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/product" + "?id=" + id);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetCategoryResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Category != null)
            {
                return response.Category;
            }
        }
        return null;
    }

    public async Task<ICollection<CategoryDTO>> GetAll(int page, int pageSize)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/product" + "?page=" + page + "&pageSize=" + pageSize);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetAllCategoryResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Categories != null)
            {
                return response.Categories;
            }
        }
        return [];
    }
}