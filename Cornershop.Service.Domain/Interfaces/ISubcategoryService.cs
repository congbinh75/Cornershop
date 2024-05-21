using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface ISubCategoryService
    {
        public Task<SubcategoryDTO?> GetById(string id);
        public Task<ICollection<SubcategoryDTO>> GetAll(int page, int pageSize);
        public Task<int> GetCount();
        public Task<SubcategoryDTO?> Add(SubcategoryDTO categoryDTO);
        public Task<SubcategoryDTO?> Update(SubcategoryDTO categoryDTO);
        public Task<bool> Remove(string id);
    }
}