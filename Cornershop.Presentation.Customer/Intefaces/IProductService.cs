using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Intefaces;

public interface IProductService
{
    public Task<ProductDTO?> GetById(string id);
    public Task<ICollection<ProductDTO>> GetAll(int page, int pageSize, string? categoryId = null, string? subcategoryId = null);
}