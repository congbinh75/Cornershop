using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Interfaces;

public interface IProductService
{
    public Task<ProductDTO?> GetById(string id);
    public Task<(ICollection<ProductDTO> products, int count)> GetAll(int page, int pageSize, string? keyword = null,
        string? categoryId = null, string? subcategoryId = null);
}