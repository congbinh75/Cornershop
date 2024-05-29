using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;

namespace Cornershop.Presentation.Customer.Services;

public class ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IProductService
{
    public async Task<ProductDTO?> GetById(string id)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/product/" + id );

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetProductResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Product != null)
            {
                return response.Product;
            }
        }
        return null;
    }

    public async Task<ICollection<ProductDTO>> GetAll(int page, int pageSize, string? categoryId = null, string? subcategoryId = null)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/product" + "?page=" + page + "&pageSize=" + pageSize + "&categoryId=" + categoryId + "&subcategoryId=" + subcategoryId);

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
}