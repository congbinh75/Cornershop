using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services;

public class AuthorService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IAuthorService
{
    public async Task<Result<AuthorDTO?, string?>> GetById(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author == null) return Constants.ERR_AUTHOR_NOT_FOUND;
        return author.Map();
    }

    public async Task<(ICollection<AuthorDTO> authors, int count)> GetAll(int page, int pageSize)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var authors = await dbContext.Authors.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedOn).ToListAsync();
        var count = dbContext.Authors.Count();
        return (authors.ConvertAll(AuthorMapper.Map)!, count);
    }

    public async Task<Result<AuthorDTO?, string?>> Add(AuthorDTO authorDTO)
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

    public async Task<Result<AuthorDTO?, string?>> Update(AuthorDTO authorDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var author = await dbContext.Authors.FirstOrDefaultAsync(c => c.Id == authorDTO.Id) ;
        if (author == null) return Constants.ERR_AUTHOR_NOT_FOUND;

        author.Name = authorDTO.Name ?? author.Name;
        author.Description = authorDTO.Description ?? author.Description;

        await dbContext.SaveChangesAsync();

        return author.Map();
    }

    public async Task<Result<bool, string?>> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var author = await dbContext.Authors.FirstOrDefaultAsync(c => c.Id == id);
        if (author == null) return Constants.ERR_AUTHOR_NOT_FOUND;
        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
