using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryDTO?> GetById(string id);
        public Task<ICollection<CategoryDTO>> GetAll(int page, int pageSize);
        public Task<int> GetCount();
        public Task<CategoryDTO?> Add(CategoryDTO categoryDTO);
        public Task<CategoryDTO?> Update(CategoryDTO categoryDTO);
        public Task<bool> Remove(string id);
    }
}