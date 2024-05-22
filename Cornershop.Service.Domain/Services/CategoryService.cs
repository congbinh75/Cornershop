using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class CategoryService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ICategoryService
    {
        public async Task<CategoryDTO?> GetById(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception(); //TO BE FIXED
            return category.Map();
        }
        
        public async Task<ICollection<CategoryDTO>> GetAll(int page, int pageSize)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var categories = await dbContext.Categories.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return categories.ConvertAll(CategoryMapper.Map);
        }

        public async Task<int> GetCount()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return dbContext.Categories.Count();
        }

        public async Task<CategoryDTO?> Add(CategoryDTO categoryDTO)
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

        public async Task<CategoryDTO?> Update(CategoryDTO categoryDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryDTO.Id) ?? throw new Exception(); //TO BE FIXED
            
            category.Name = categoryDTO.Name ?? category.Name;
            category.Description = categoryDTO.Description ?? category.Description;

            await dbContext.SaveChangesAsync();

            return category.Map();
        }

        public async Task<bool> Remove(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}