using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class PublisherService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IPublisherService
{
    public async Task<Result<PublisherDTO?, string?>> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var publisher = await dbContext.Publishers.FirstOrDefaultAsync(a => a.Id == id);
        if (publisher == null) return Constants.ERR_PUBLISHER_NOT_FOUND;
        return publisher.Map();
    }

    public async Task<(ICollection<PublisherDTO> publishers, int count)> GetAll(int page, int pageSize)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var publishers = await dbContext.Publishers.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedOn).ToListAsync();
        var count = dbContext.Publishers.Count();
        return (publishers.ConvertAll(PublisherMapper.Map)!, count);
    }

    public async Task<Result<PublisherDTO?, string?>> Add(PublisherDTO publisherDTO)
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

    public async Task<Result<PublisherDTO?, string?>> Update(PublisherDTO publisherDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var publisher = await dbContext.Publishers.FirstOrDefaultAsync(c => c.Id == publisherDTO.Id);
        if (publisher == null) return Constants.ERR_PUBLISHER_NOT_FOUND;

        publisher.Name = publisherDTO.Name ?? publisher.Name;
        publisher.Description = publisherDTO.Description ?? publisher.Description;

        await dbContext.SaveChangesAsync();

        return publisher.Map();
    }

    public async Task<Result<bool, string?>> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var publisher = await dbContext.Publishers.FirstOrDefaultAsync(c => c.Id == id);
        if (publisher == null) return Constants.ERR_PUBLISHER_NOT_FOUND;
        dbContext.Publishers.Remove(publisher);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
