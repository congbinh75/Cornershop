using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class SubcategoryService(IDbContextFactory<CornershopDbContext> dbContextFactory) : ISubCategoryService
    {
        public async Task<SubcategoryDTO?> GetById(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception(); //TO BE FIXED
            return Mapper.Map(result);
        }

        public async Task<ICollection<SubcategoryDTO>> GetAll(int page, int pageSize)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subcategories.Skip(page * pageSize).Take(pageSize).ToListAsync();
            return result.ConvertAll(Mapper.Map);
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
            return Mapper.Map(subcategory);
        }

        public async Task<SubcategoryDTO?> Update(SubcategoryDTO subcategoryDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Category.Id) ?? throw new Exception(); //TO BE FIXED
            var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == subcategoryDTO.Category.Id) ?? throw new Exception();
            
            subcategory.Name = subcategoryDTO.Name ?? subcategory.Name;
            subcategory.Description = subcategoryDTO.Description ?? subcategory.Description;
            subcategory.Category = category;

            await dbContext.SaveChangesAsync();

            return Mapper.Map(subcategory);
        }

        public async Task<bool> Remove(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == id);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}