using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Microsoft.Net.Http.Headers;

namespace Cornershop.Presentation.Customer.Services;

public class ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IProductService
{
    public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
    }
    
    public async Task<ICollection<ProductDTO>> GetAll(int page, int pageSize, bool isHiddenIncluded = false)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            configuration["Service:BaseUrl"] + "/product" +  "?page=" + page + "&pageSize=" + pageSize)
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
            
            var result = await JsonSerializer.DeserializeAsync <ICollection<ProductDTO>>(contentStream);
            
            if (result != null) 
            {
                return result;
            }
        }
        return [];
    }

    public Task<ICollection<ProductDTO>> GetAllByCategory(string categoryId, int page, int pageSize, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ProductDTO>> GetAllBySubcategory(string subcategoryId, int page, int pageSize, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
    }
}