using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDTO?> GetById(string id, bool isVisible = false);
        public Task<ICollection<ProductDTO>> GetList(int page, int pageSize, bool isVisible = false);
        public Task<ICollection<ProductDTO>> GetListByCategory(string categoryId, int page, int pageSize, bool isVisible);
        public Task<ProductDTO?> Add(ProductDTO productDTO);
        public Task<ProductDTO?> Update(ProductDTO productDTO);
        public Task<bool> Remove(string id);
        public Task<bool> AddRating(string id, int rating);
        public Task<bool> RemoveRating(string id);
    }
}