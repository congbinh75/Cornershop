using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class ProductService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IProductService
    {
        public async Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            if (isHiddenIncluded) 
            {
                var product = await dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync() ?? throw new Exception(); //TO BE FIXED
                return Mapper.Map(product);
            }
            else
            {
                var product = await dbContext.Products.Where(p => p.Id == id && p.IsVisible == true).FirstOrDefaultAsync() ?? throw new Exception(); //TO BE FIXED
                return Mapper.Map(product); 
            }
        }

        public async Task<ICollection<ProductDTO>> GetAll(int page, int pageSize, bool isHiddenIncluded = false)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            if (isHiddenIncluded) 
            {
                var products = await dbContext.Products.Skip(page * pageSize).Take(pageSize).ToListAsync() ?? throw new Exception(); //TO BE FIXED
                return products.ConvertAll(Mapper.Map);
            }
            else
            {
                var products = await dbContext.Products.Where(p => p.IsVisible == true)
                    .Skip(page * pageSize).Take(pageSize).ToListAsync() ?? throw new Exception(); //TO BE FIXED
                return products.ConvertAll(Mapper.Map);
            }
        }

        public async Task<ICollection<ProductDTO>> GetAllBySubcategory(string subcategoryId, int page, int pageSize, bool isHiddenIncluded = false)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            if (isHiddenIncluded) 
            {
                var products = await dbContext.Products.Where(p => p.Subcategory.Id == subcategoryId)
                    .Skip(page * pageSize).Take(pageSize).ToListAsync() ?? throw new Exception(); //TO BE FIXED
                return products.ConvertAll(Mapper.Map);
            }
            else
            {
                var products = await dbContext.Products.Where(p => p.Subcategory.Id == subcategoryId && p.IsVisible == true)
                    .Skip(page * pageSize).Take(pageSize).ToListAsync() ?? throw new Exception(); //TO BE FIXED
                return products.ConvertAll(Mapper.Map);
            }
        }

        public async Task<ProductDTO?> Add(ProductDTO productDTO)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(s => s.Id == productDTO.Subcategory.Id) ?? throw new Exception(); //TO BE FIXED
            var authorIds = productDTO.Authors.Select(a => a.Id).ToList();
            var authors = await dbContext.Authors.Where(a => authorIds.Contains(a.Id)).ToListAsync() ?? throw new Exception();
            var publisher = await dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == productDTO.Publisher.Id) ?? throw new Exception(); //TO BE FIXED
            var product = new Product
            {
                Name = productDTO.Name,
                Code = productDTO.Code,
                Description = productDTO.Description,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id,
                Price = productDTO.Price,
                OriginalPrice = productDTO.OriginalPrice,
                Width = productDTO.Width,
                Length = productDTO.Length,
                Height = productDTO.Height,
                Pages = productDTO.Pages,
                Format = productDTO.Format,
                Stock = productDTO.Stock,
                PublishedYear = productDTO.PublishedYear,
                Rating = 0,
                IsVisible = false,
                Authors = authors,
                Publisher = publisher
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return Mapper.Map(product);
        }

        public async Task<ProductDTO?> Update(ProductDTO productDTO)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productDTO.Id) ?? throw new Exception(); //TO BE FIXED
            
            product.Name = productDTO.Name ?? product.Name;
            product.Description = productDTO.Description ?? product.Description;
            product.Code = productDTO.Code ?? product.Code;
            product.Price = productDTO.Price;
            product.Subcategory = Mapper.Map(productDTO.Subcategory);
            product.SubcategoryId = productDTO.Subcategory.Id;
            product.OriginalPrice = productDTO.OriginalPrice;
            product.Width = productDTO.Width;
            product.Length = productDTO.Length;
            product.Height = productDTO.Height;
            product.Pages = productDTO.Pages;
            product.Format = productDTO.Format;
            product.Stock = productDTO.Stock;
            product.PublishedYear = productDTO.PublishedYear;
            product.ImagesUrls = productDTO.ImagesUrls ?? product.ImagesUrls;
            product.Rating = productDTO.Rating;
            product.Reviews = productDTO.Reviews.Select(Mapper.Map).ToList() ?? product.Reviews;
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
    }
}