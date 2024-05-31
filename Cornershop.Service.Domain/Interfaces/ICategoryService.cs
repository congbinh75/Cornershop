using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface ICategoryService
    {
        public Task<Result<CategoryDTO?, string?>> GetById(string id);
        public Task<(ICollection<CategoryDTO> categories, int count)> GetAll(int page, int pageSize);
        public Task<Result<CategoryDTO?, string?>> Add(CategoryDTO categoryDTO);
        public Task<Result<CategoryDTO?, string?>> Update(CategoryDTO categoryDTO);
        public Task<Result<bool, string?>> Remove(string id);
    }
}