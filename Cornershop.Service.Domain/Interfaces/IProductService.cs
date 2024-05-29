using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false);
        public Task<(ICollection<ProductDTO> products, int count)> GetAll(int page, int pageSize, bool isHiddenIncluded = false, 
            string? categoryId = null, string? subcategoryId = null, bool? isOrderedByPriceAscending = null);
        public Task<ProductDTO?> Add(ProductDTO productDTO);
        public Task<ProductDTO?> Update(ProductDTO productDTO);
        public Task<bool> Remove(string id);
    }
}