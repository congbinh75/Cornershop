using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class SubcategoryService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ISubcategoryService
{
    public async Task<Result<SubcategoryDTO?, string?>> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var subcategory = await dbContext.Subcategories
            .Where(c => c.Id == id)
            .Include(c => c.Category)
            .FirstOrDefaultAsync();
        if (subcategory == null) return Constants.ERR_SUBCATEGORY_NOT_FOUND;
        return subcategory.Map();
    }

    public async Task<(ICollection<SubcategoryDTO> subcategories, int count)> GetAll(int page, int pageSize, string? categoryId = null)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        IQueryable<Subcategory> query = dbContext.Subcategories;
        if (!string.IsNullOrEmpty(categoryId)) query = query.Where(c => c.Category.Id == categoryId);

        var subcategories = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .OrderBy(a => a.CreatedOn).Include(s => s.Category).ToListAsync();
        var count = query.Count();

        return (subcategories.ConvertAll(SubcategoryMapper.Map)!, count);
    }

    public async Task<Result<SubcategoryDTO?, string?>> Add(SubcategoryDTO subcategoryDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Category.Id);
        if (category == null) return Constants.ERR_SUBCATEGORY_NOT_FOUND;
        var subcategory = new Subcategory
        {
            Name = subcategoryDTO.Name,
            Description = subcategoryDTO.Description,
            Category = category
        };
        await dbContext.Subcategories.AddAsync(subcategory);
        await dbContext.SaveChangesAsync();
        return subcategory.Map();
    }

    public async Task<Result<SubcategoryDTO?, string?>> Update(SubcategoryDTO subcategoryDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Category.Id);
        if (category == null) return Constants.ERR_CATEGORY_NOT_FOUND;
        var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Id);
        if (subcategory == null) return Constants.ERR_SUBCATEGORY_NOT_FOUND;

        subcategory.Name = subcategoryDTO.Name ?? subcategory.Name;
        subcategory.Description = subcategoryDTO.Description ?? subcategory.Description;
        subcategory.Category = category;

        await dbContext.SaveChangesAsync();

        return subcategory.Map();
    }

    public async Task<Result<bool, string?>> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == id);
        if (subcategory == null) return Constants.ERR_SUBCATEGORY_NOT_FOUND;
        dbContext.Subcategories.Remove(subcategory);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
