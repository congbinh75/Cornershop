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

    public Task<ICollection<SubcategoryDTO>> GetAllByCategory(string categoryId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}