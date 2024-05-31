using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces;

public interface ISubcategoryService
{
    public Task<Result<SubcategoryDTO?, string?>> GetById(string id);
    public Task<(ICollection<SubcategoryDTO> subcategories, int count)> GetAll(int page, int pageSize, string? categoryId = null);
    public Task<Result<SubcategoryDTO?, string?>> Add(SubcategoryDTO categoryDTO);
    public Task<Result<SubcategoryDTO?, string?>> Update(SubcategoryDTO categoryDTO);
    public Task<Result<bool, string?>> Remove(string id);
}
