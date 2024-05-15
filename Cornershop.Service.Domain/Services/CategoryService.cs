using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class CategoryService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ICategoryService
    {
        public async Task<CategoryDTO?> Add(CategoryDTO categoryDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var category = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };
            await dbContext.Categories.AddAsync(category);
            return Mapper.Map(category);
        }

        public async Task<ICollection<CategoryDTO>> GetAll()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Categories.Take(100).ToListAsync();
            return result.ConvertAll(Mapper.Map);
        }

        public async Task<CategoryDTO?> GetById(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception(); //TO BE FIXED
            return Mapper.Map(result);
        }

        public async Task<bool> Remove(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CategoryDTO?> Update(CategoryDTO categoryDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryDTO.Id) ?? throw new Exception(); //TO BE FIXED
            
            category.Name = categoryDTO.Name ?? category.Name;
            category.Description = categoryDTO.Description ?? category.Description;

            await dbContext.SaveChangesAsync();

            return Mapper.Map(category);
        }
    }
}