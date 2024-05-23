using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class PublisherService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IPublisherService
    {
        public async Task<PublisherDTO?> GetById(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var publisher = await dbContext.Publishers.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception(); //TO BE FIXED
            return publisher.Map();
        }

        public async Task<ICollection<PublisherDTO>> GetAll(int page, int pageSize)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var publishers = await dbContext.Publishers.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return publishers.ConvertAll(PublisherMapper.Map);
        }

        public async Task<int> GetCount()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return dbContext.Publishers.Count();
        }

        public async Task<PublisherDTO?> Add(PublisherDTO publisherDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var publisher = new Publisher
            {
                Name = publisherDTO.Name,
                Description = publisherDTO.Description
            };
            await dbContext.Publishers.AddAsync(publisher);
            await dbContext.SaveChangesAsync();
            return publisher.Map();
        }

        public async Task<PublisherDTO?> Update(PublisherDTO publisherDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var publisher = await dbContext.Publishers.FirstOrDefaultAsync(c => c.Id == publisherDTO.Id) ?? throw new Exception(); //TO BE FIXED
            
            publisher.Name = publisherDTO.Name ?? publisher.Name;
            publisher.Description = publisherDTO.Description ?? publisher.Description;

            await dbContext.SaveChangesAsync();

            return publisher.Map();
        }

        public async Task<bool> Remove(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Publishers.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception();
            dbContext.Publishers.Remove(result);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}