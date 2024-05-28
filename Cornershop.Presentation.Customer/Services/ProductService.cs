using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Microsoft.Net.Http.Headers;

namespace Cornershop.Presentation.Customer.Services;

public class ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IProductService
{
    public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<ProductDTO>> GetAll(int page, int pageSize)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "https://localhost:5000/api/product?page=1&pageSize=15")
        {
            Headers =
            {
                { HeaderNames.Accept, "application/json" },
                { HeaderNames.UserAgent, "CostumerClient" }
            }
        };

        var httpClient = httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<GetAllProductResponse>(contentStream);

            if (result != null && result.Products != null) 
            {
                return result.Products;
            }
        }
        return [];
    }

    public Task<ICollection<ProductDTO>> GetAllByCategory(string categoryId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ProductDTO>> GetAllBySubcategory(string subcategoryId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}