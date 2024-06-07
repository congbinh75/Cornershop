using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Interfaces;

public interface ISubcategoryService
{
    public Task<ICollection<SubcategoryDTO>> GetAllByCategory(string categoryId, int page, int pageSize);
}