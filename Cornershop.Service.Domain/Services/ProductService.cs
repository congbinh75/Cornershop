using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class ProductService(IDbContextFactory<CornershopDbContext> dbContextFactory, ITokenInfoProvider tokenInfoProvider) : IProductService
    {
        public async Task<ProductDTO?> GetById(string id, bool isVisible = false)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var product = await dbContext.Products.Where(p => p.Id == id && p.IsVisible == isVisible).FirstOrDefaultAsync()
                ?? throw new Exception(); //TO BE FIXED
            return Mapper.Map(product);
        }

        public async Task<ICollection<ProductDTO>> GetList(int page, int pageSize, bool isVisible = false)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var products = await dbContext.Products.Where(p => p.IsVisible == isVisible).Skip(page * pageSize).Take(pageSize).ToListAsync();
            return products.ConvertAll(Mapper.Map);
        }

        public async Task<ICollection<ProductDTO>> GetListByCategory(string categoryId, int page, int pageSize, bool isVisible)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var products = await dbContext.Products.Where(x => x.Category.Id == categoryId && x.IsVisible == isVisible)
                .Skip(page * pageSize).Take(pageSize).ToListAsync();
            return products.ConvertAll(Mapper.Map);
        }

        public async Task<ProductDTO?> Add(ProductDTO productDTO)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == productDTO.Category.Id) ?? throw new Exception(); //TO BE FIXED
            var product = new Product
            {
                Name = productDTO.Name,
                Code = productDTO.Code,
                Description = productDTO.Description,
                Category = category,
                Price = productDTO.Price,
                Rating = 0,
                IsVisible = false
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return Mapper.Map(product);
        }

        public async Task<ProductDTO?> Update(ProductDTO productDTO)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productDTO.Id) ?? throw new Exception(); //TO BE FIXED
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == productDTO.Category.Id) ?? product.Category;
            
            product.Name = productDTO.Name ?? product.Name;
            product.Description = productDTO.Description ?? product.Description;
            product.Category = category;
            product.Code = productDTO.Code ?? product.Code;
            product.Price = productDTO.Price;
            product.ImagesUrls = productDTO.ImagesUrls ?? product.ImagesUrls;
            product.Rating = productDTO.Rating;
            product.RatingVotes = productDTO.RatingVotes.Select(Mapper.Map).ToList() ?? product.RatingVotes;
            product.IsVisible = productDTO.IsVisible;

            await dbContext.SaveChangesAsync();
            return Mapper.Map(product);
        }

        public async Task<bool> Remove(string id)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception(); //TO BE FIXED
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddRating(string id, int rating)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception(); //TO BE FIXED
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == tokenInfoProvider.Id) ?? throw new Exception(); //TO BE FIXED
            var ratingVote = new RatingVote
            {
                Product = product,
                ProductId = product.Id,
                User = user,
                UserId = user.Id,
                Rate = rating
            };
            await dbContext.RatingVotes.AddAsync(ratingVote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRating(string id)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var ratingVote = await dbContext.RatingVotes.Where(x => x.ProductId == id && x.UserId == tokenInfoProvider.Id).FirstOrDefaultAsync() ?? throw new Exception(); //TO BE FIXED
            dbContext.RatingVotes.Remove(ratingVote);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}