using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;

namespace Cornershop.Presentation.Customer.Services;

public class ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IProductService
{
    public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<ProductDTO>> GetAll(int page, int pageSize)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/product" + "?page=" + page + "&pageSize=" + pageSize);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetAllProductResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Products != null)
            {
                return response.Products;
            }
        }
        return [];
    }

    public async Task<ICollection<ProductDTO>> GetAllByCategory(string categoryId, int page, int pageSize)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/product" + "?page=" + page + "&pageSize=" + pageSize);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetAllProductResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Products != null)
            {
                return response.Products;
            }
        }
        return [];
    }

    public Task<ICollection<ProductDTO>> GetAllBySubcategory(string subcategoryId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}