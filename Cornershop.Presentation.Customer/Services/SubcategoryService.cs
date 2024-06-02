using System.Text.Json;
using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;

namespace Cornershop.Presentation.Customer.Services;

public class SubcategoryService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ISubcategoryService
{
    public Task<SubcategoryDTO?> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<SubcategoryDTO>> GetAllByCategory(string categoryId, int page, int pageSize)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/subcategory" + "?page=" + page + "&pageSize=" + pageSize 
            + "&categoryId=" + categoryId);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetAllSubcategoryResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Subcategories != null)
            {
                return response.Subcategories;
            }
        }
        return [];
    }
}