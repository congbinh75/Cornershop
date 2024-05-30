using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class SubcategoryService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ISubCategoryService
{
    public async Task<SubcategoryDTO?> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var subcategory = await dbContext.Subcategories
            .Where(c => c.Id == id)
            .Include(c => c.Category)
            .FirstOrDefaultAsync() ?? throw new Exception(); //TO BE FIXED
        return subcategory.Map();
    }

    public async Task<ICollection<SubcategoryDTO>> GetAll(int page, int pageSize)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var subcategories = await dbContext.Subcategories.Skip((page - 1) * pageSize).Take(pageSize)
            .OrderByDescending(a => a.CreatedOn).Include(s => s.Category).ToListAsync();
        return subcategories.ConvertAll(SubcategoryMapper.Map);
    }

    public async Task<ICollection<SubcategoryDTO>> GetAllByCategory(int page, int pageSize, string categoryId)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var subcategories = await dbContext.Subcategories.Where(s => s.Category.Id == categoryId)
            .Skip((page - 1) * pageSize).Take(pageSize).Include(s => s.Category).ToListAsync();
        return subcategories.ConvertAll(SubcategoryMapper.Map);
    }

    public async Task<int> GetCount()
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return dbContext.Subcategories.Count();
    }

    public async Task<SubcategoryDTO?> Add(SubcategoryDTO subcategoryDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Category.Id) ?? throw new Exception(); //TO BE FIXED
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

    public async Task<SubcategoryDTO?> Update(SubcategoryDTO subcategoryDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Category.Id) ?? throw new Exception(); //TO BE FIXED
        var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Id) ?? throw new Exception();

        subcategory.Name = subcategoryDTO.Name ?? subcategory.Name;
        subcategory.Description = subcategoryDTO.Description ?? subcategory.Description;
        subcategory.Category = category;

        await dbContext.SaveChangesAsync();

        return subcategory.Map();
    }

    public async Task<bool> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var result = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception();
        dbContext.Subcategories.Remove(result);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
