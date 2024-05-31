using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IProductService
    {
        public Task<Result<ProductDTO?, string?>> GetById(string id, bool isHiddenIncluded = false);
        public Task<(ICollection<ProductDTO> products, int count)> GetAll(int page, int pageSize, bool isHiddenIncluded = false, 
            string? categoryId = null, string? subcategoryId = null, bool? isOrderedByPriceAscending = null);
        public Task<Result<ProductDTO?, string?>> Add(ProductDTO productDTO);
        public Task<Result<ProductDTO?, string?>> Update(ProductDTO productDTO);
        public Task<Result<bool, string?>> Remove(string id);
    }
}