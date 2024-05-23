using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class AuthorService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IAuthorService
    {
        public async Task<AuthorDTO?> GetById(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception(); //TO BE FIXED
            return author.Map();
        }

        public async Task<ICollection<AuthorDTO>> GetAll(int page, int pageSize)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var authors = await dbContext.Authors.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return authors.ConvertAll(AuthorMapper.Map);
        }

        public async Task<int> GetCount()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return dbContext.Authors.Count();
        }

        public async Task<AuthorDTO?> Add(AuthorDTO authorDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var author = new Author
            {
                Name = authorDTO.Name,
                Description = authorDTO.Description
            };
            await dbContext.Authors.AddAsync(author);
            await dbContext.SaveChangesAsync();
            return author.Map();
        }

        public async Task<AuthorDTO?> Update(AuthorDTO authorDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var author = await dbContext.Authors.FirstOrDefaultAsync(c => c.Id == authorDTO.Id) ?? throw new Exception(); //TO BE FIXED
            
            author.Name = authorDTO.Name ?? author.Name;
            author.Description = authorDTO.Description ?? author.Description;

            await dbContext.SaveChangesAsync();

            return author.Map();
        }

        public async Task<bool> Remove(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Authors.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception();
            dbContext.Authors.Remove(result);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}