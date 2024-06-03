using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Cornershop.Service.Common;

namespace Cornershop.Service.Domain.Services;

public class CategoryService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ICategoryService
{
    public async Task<Result<CategoryDTO?, string?>> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return Constants.ERR_CATEGORY_NOT_FOUND;
        return category.Map();
    }

    public async Task<(ICollection<CategoryDTO> categories, int count)> GetAll(int page, int pageSize)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var categories = await dbContext.Categories.Skip((page - 1) * pageSize).Take(pageSize).OrderBy(a => a.CreatedOn).ToListAsync();
        var count = dbContext.Categories.Count();
        return (categories.ConvertAll(CategoryMapper.Map)!, count);
    }

    public async Task<Result<CategoryDTO?, string?>> Add(CategoryDTO categoryDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = new Category
        {
            Name = categoryDTO.Name,
            Description = categoryDTO.Description
        };
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();
        return category.Map();
    }

    public async Task<Result<CategoryDTO?, string?>> Update(CategoryDTO categoryDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryDTO.Id);
        if (category == null) return Constants.ERR_CATEGORY_NOT_FOUND;

        category.Name = categoryDTO.Name ?? category.Name;
        category.Description = categoryDTO.Description ?? category.Description;

        await dbContext.SaveChangesAsync();

        return category.Map();
    }

    public async Task<Result<bool, string?>> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return Constants.ERR_CATEGORY_NOT_FOUND;
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
