using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Intefaces;

public interface IProductService
{
    public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false);
    public Task<ICollection<ProductDTO>> GetAll(int page, int pageSize);
    public Task<ICollection<ProductDTO>> GetAllByCategory(string categoryId, int page, int pageSize);
    public Task<ICollection<ProductDTO>> GetAllBySubcategory(string subcategoryId, int page, int pageSize);
}