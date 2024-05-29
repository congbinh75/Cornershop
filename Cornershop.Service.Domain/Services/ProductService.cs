using Cornershop.Shared.DTOs;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Cornershop.Service.Domain.Mappers;
using Cornershop.Service.Common.Functions;
using Cornershop.Service.Common;

namespace Cornershop.Service.Domain.Services;

public class ProductService(IDbContextFactory<CornershopDbContext> dbContextFactory) : IProductService
{
    public async Task<ProductDTO?> GetById(string id, bool isHiddenIncluded = false)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        if (isHiddenIncluded)
        {
            var product = await dbContext.Products.Where(p => p.Id == id)
                .Include(p => p.Author)
                .Include(p => p.Publisher)
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync() ?? throw new Exception(); //TO BE FIXED
            return product.Map();
        }
        else
        {
            var product = await dbContext.Products.Where(p => p.Id == id && p.IsVisible == true)
                .Include(p => p.Author)
                .Include(p => p.Publisher)
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync() ?? throw new Exception(); //TO BE FIXED
            return product.Map();
        }
    }

    public async Task<(ICollection<ProductDTO> products, int count)> GetAll(int page, int pageSize, bool isHiddenIncluded = false, 
        string? categoryId = null, string? subcategoryId = null, bool? isOrderedByPriceAscending = null)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        if (isHiddenIncluded)
        {
            var query = dbContext.Products
                .Where(p => (categoryId == null || p.Subcategory.Category.Id == categoryId)
                    && (subcategoryId == null || p.Subcategory.Id == subcategoryId))
                .Include(p => p.Author)
                .Include(p => p.Publisher)
                .Include(p => p.Subcategory).ThenInclude(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(a => a.CreatedOn);

            if (isOrderedByPriceAscending != null) 
            {
                query = (bool)isOrderedByPriceAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
            }
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (products.ConvertAll(ProductMapper.Map), query.Count());
        }
        else
        {
            var query = dbContext.Products
                .Where(p => p.IsVisible == true 
                    && (categoryId == null || p.Subcategory.Category.Id == categoryId)
                    && (subcategoryId == null || p.Subcategory.Id == subcategoryId))
                .Include(p => p.Author)
                .Include(p => p.Publisher)
                .Include(p => p.Subcategory).ThenInclude(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(a => a.CreatedOn);
            
            if (isOrderedByPriceAscending != null) 
            {
                query = (bool)isOrderedByPriceAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
            }
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (products.ConvertAll(ProductMapper.Map), query.Count());
        }
    }

    public async Task<ProductDTO?> Add(ProductDTO productDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(s => s.Id == productDTO.SubcategoryId) ?? throw new Exception(); //TO BE FIXED
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Id == productDTO.AuthorId) ?? throw new Exception();
        var publisher = await dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == productDTO.PublisherId) ?? throw new Exception(); //TO BE FIXED

        var product = new Product
        {
            Name = productDTO.Name,
            Code = await GenerateProductCode(),
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
            IsVisible = productDTO.IsVisible,
            Author = author,
            Publisher = publisher
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var mainFile = productDTO.ProductImages.FirstOrDefault(p => p.IsMainImage == true);
        var mainImage = new ProductImage
        {
            Product = product,
            ProductId = product.Id,
            ImageUrl = mainFile?.ImageUrl ?? "",
            IsMainImage = true
        };
        await dbContext.ProductImages.AddAsync(mainImage);
        productDTO.ProductImages.Remove(mainFile);

        foreach (var imageFile in productDTO.ProductImages)
        {
            var image = new ProductImage
            {
                Product = product,
                ProductId = product.Id,
                ImageUrl = imageFile.ImageUrl,
                IsMainImage = false
            };
            await dbContext.ProductImages.AddAsync(image);
        }

        await dbContext.SaveChangesAsync();
        return product.Map();
    }

    public async Task<ProductDTO?> Update(ProductDTO productDTO)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var product = await dbContext.Products
            .Include(p => p.Author)
            .Include(p => p.Publisher)
            .Include(p => p.Subcategory)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == productDTO.Id) ?? throw new Exception(); //TO BE FIXED
        var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(p => p.Id == productDTO.SubcategoryId);
        var author = await dbContext.Authors.FirstOrDefaultAsync(p => p.Id == productDTO.AuthorId);
        var publisher = await dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == productDTO.PublisherId);

        product.Name = productDTO.Name ?? product.Name;
        product.Description = productDTO.Description ?? product.Description;
        product.Code = productDTO.Code ?? product.Code;
        product.Price = productDTO.Price;
        product.Subcategory = subcategory ?? product.Subcategory;
        product.OriginalPrice = productDTO.OriginalPrice;
        product.Width = productDTO.Width;
        product.Length = productDTO.Length;
        product.Height = productDTO.Height;
        product.Pages = productDTO.Pages;
        product.Format = productDTO.Format;
        product.Stock = productDTO.Stock;
        product.PublishedYear = productDTO.PublishedYear;
        product.Rating = productDTO.Rating;
        product.IsVisible = productDTO.IsVisible;
        product.Author = author ?? product.Author;
        product.Publisher = publisher ?? product.Publisher;

        var deletedProductImages = new List<ProductImage>();
        foreach (var item in product.ProductImages)
        {
            if (!productDTO.ProductImagesIds.Contains(item.Id))
            {
                deletedProductImages.Add(item);
            }
        }
        dbContext.ProductImages.RemoveRange(deletedProductImages);

        foreach (var imageFile in productDTO.ProductImages)
        {
            var image = new ProductImage
            {
                Product = product,
                ProductId = product.Id,
                ImageUrl = imageFile.ImageUrl,
                IsMainImage = imageFile.IsMainImage
            };
            await dbContext.ProductImages.AddAsync(image);
        }

        await dbContext.SaveChangesAsync();
        return product.Map();
    }

    public async Task<bool> Remove(string id)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception(); //TO BE FIXED
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<string> GenerateProductCode()
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var productCode = "";
        do
        {
            productCode = Constants.ProductCodePrefix + Functions.GenerateRandomString(Constants.PostfixStringLength);
        }
        while (await dbContext.Products.FirstOrDefaultAsync(p => p.Code == productCode) != null);
        return productCode;
    }
}
