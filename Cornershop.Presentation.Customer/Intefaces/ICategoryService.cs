using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Intefaces;

public interface ICategoryService
{
    public Task<CategoryDTO?> GetById(string id);
    public Task<ICollection<CategoryDTO>> GetAll(int page, int pageSize);
}