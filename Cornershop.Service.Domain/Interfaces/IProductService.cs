using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false);
        public Task<ICollection<ProductDTO>> GetAll(int page, int pageSize, bool isHiddenIncluded = false);
        public Task<int> GetCount();
        public Task<ICollection<ProductDTO>> GetAllBySubcategory(string subcategoryId, int page, int pageSize, bool isHiddenIncluded = false);
        public Task<ProductDTO?> Add(ProductDTO productDTO);
        public Task<ProductDTO?> Update(ProductDTO productDTO);
        public Task<bool> Remove(string id);
    }
}