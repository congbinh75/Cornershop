using Cornershop.Presentation.Customer.Intefaces;
using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Services;

public class ProductService : IProductService
{
    public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
    }
    
    public Task<ICollection<ProductDTO>> GetAll(int page, int pageSize, bool isHiddenIncluded = false)
    {
        throw new NotImplementedException();
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