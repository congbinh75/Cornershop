using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Cornershop.Service.Common.Enums;

namespace Cornershop.Tests.Service.Domain;

public class ProductServiceTests
{
    #region TestData
    private readonly List<Product> products =
    [
        new Product
        {
            Id = "1",
            Name = "Name",
            Code = "Code",
            Description = "Description",
            Subcategory = new Subcategory
            {
                Id = "Subcategory1",
                Name = "Name",
                Description = "Description",
                Category = new Category
                {
                    Id = "Category1",
                    Name = "Category",
                    Description = "Description"
                }
            },
            SubcategoryId = "Subcategory1",
            Author = new Author
            {
                Id = "Author1",
                Name = "Name",
                Description = "Description",
            },
            Publisher = new Publisher
            {
                Id = "Publisher1",
                Name = "Name",
                Description = "Description",
            },
            Price = 0,
            OriginalPrice = 0,
            Width = 0,
            Length = 0,
            Height = 0,
            Rating = 0,
            Pages = 0,
            Format = (int)Format.Paperback,
            Stock = 0,
            PublishedYear = 0,
            IsVisible = true
        },
        new Product
        {
            Id = "2",
            Name = "Name",
            Code = "Code",
            Description = "Description",
            Subcategory = new Subcategory
            {
                Id = "Subcategory2",
                Name = "Name",
                Description = "Description",
                Category = new Category
                {
                    Id = "Category2",
                    Name = "Category",
                    Description = "Description"
                }
            },
            SubcategoryId = "Subcategory2",
            Author = new Author
            {
                Id = "Author2",
                Name = "Name",
                Description = "Description",
            },
            Publisher = new Publisher
            {
                Id = "Publisher2",
                Name = "Name",
                Description = "Description",
            },
            Price = 0,
            OriginalPrice = 0,
            Width = 0,
            Length = 0,
            Height = 0,
            Rating = 0,
            Pages = 0,
            Format = (int)Format.Paperback,
            Stock = 0,
            PublishedYear = 0,
            IsVisible = false
        }
    ];
    #endregion

    #region UtilsMethods
    private async Task<Mock<IDbContextFactory<CornershopDbContext>>> CreateMockDbContextFactoryAsync()
    {
        var mockTokenInfoProvider = new Mock<ITokenInfoProvider>();
        mockTokenInfoProvider.Setup(t => t.Id).Returns("TestUser");

        var options = new DbContextOptionsBuilder<CornershopDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;

        using (var context = new CornershopDbContext(options, mockTokenInfoProvider.Object))
        {
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static ProductDTO GenerateSampleProductDTO()
    {
        return new ProductDTO
        {
            Id = "1",
            Name = "Name",
            Description = "Description",
            SubcategoryId = "Subcategory1",
            Price = 0,
            OriginalPrice = 0,
            Width = 0,
            Length = 0,
            Height = 0,
            Pages = 0,
            Format = (int)Format.Paperback,
            Stock = 0,
            PublishedYear = 0,
            Rating = 0,
            IsVisible = false,
            AuthorId = "Author1",
            PublisherId = "Publisher1",
            ProductImages =
            [
                new ProductImageDTO
                {
                    ImageUrl = "url1",
                    IsMainImage = true
                },
                new ProductImageDTO
                {
                    ImageUrl = "url2",
                    IsMainImage = false
                }
            ]
        };
    }
    #endregion

    [Fact]
    public async Task GetById_HiddenNotIncluded_ProductExists_ReturnsProduct()
    {
        // Arrange
        var productId = "1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.GetById(productId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(productId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_HiddenIncluded_ProductExists_ReturnsProduct()
    {
        // Arrange
        var productId = "2";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.GetById(productId, true);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(productId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_HiddenNotIncluded_ProductNotFound_ReturnsError()
    {
        // Arrange
        var productId = "2";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.GetById(productId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PRODUCT_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetById_HiddenIncluded_ProductNotFound_ReturnsError()
    {
        // Arrange
        var productId = "3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.GetById(productId, true);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PRODUCT_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetAll_HiddenIncluded_ReturnsProducts()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.GetAll(1, 10, true);

        // Assert
        Assert.NotNull(result.products);
        Assert.Equal(products.Count, result.products.Count);
        Assert.Equal(products.Count, result.count);
    }

    [Fact]
    public async Task GetAll_WithAllCriterias_HiddenIncluded_ReturnsProducts()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);
        var sampleProduct = products.FirstOrDefault(p => p.Id == "1");

        // Act
        var result = await productService.GetAll(1, 10, true, sampleProduct!.Name, sampleProduct.Subcategory.Category.Id, sampleProduct.Subcategory.Id, true);

        // Assert
        Assert.NotNull(result.products);
        Assert.Single(result.products);
        Assert.Equal(1, result.count);
        Assert.Equal(sampleProduct.Id, result.products.First().Id);
    }

    [Fact]
    public async Task GetAll_HiddenNotIncluded_ReturnsProducts()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.GetAll(1, 10);

        // Assert
        Assert.NotNull(result.products);
        Assert.Equal(products.Where(p => p.IsVisible == true).ToList().Count, result.products.Count);
    }

    [Fact]
    public async Task Add_ReturnProduct()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        var productName = productDTO.Name;

        // Act
        var result = await productService.Add(productDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(productName, result.Value.Name);
    }

    [Fact]
    public async Task Add_SubcategoryNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.SubcategoryId = "3";

        // Act
        var result = await productService.Add(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_SUBCATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Add_AuthorNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.AuthorId = "3";

        // Act
        var result = await productService.Add(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_AUTHOR_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Add_PublisherNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.PublisherId = "3";

        // Act
        var result = await productService.Add(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PUBLISHER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Update_ReturnProduct()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        var subcategoryId = "Subcategory2";
        var authorId = "Author2";
        var publisherId = "Publisher2";
        productDTO.SubcategoryId = subcategoryId;
        productDTO.AuthorId = authorId;
        productDTO.PublisherId = publisherId;
        productDTO.ProductImages =
        [
            new() {
                Id = "2",
                ImageUrl = "url2",
                IsMainImage = false
            },
            new() {
                ImageUrl = "url3",
                IsMainImage = true
            }
        ];

        // Act
        var result = await productService.Update(productDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(productDTO.Id, result.Value.Id);
        Assert.Equal(productDTO.SubcategoryId, result.Value.Subcategory.Id);
        Assert.Equal(productDTO.AuthorId, result.Value.Author.Id);
        Assert.Equal(productDTO.PublisherId, result.Value.Publisher.Id);
        Assert.True(productDTO.ProductImages.Count == result.Value.ProductImages.Count);
    }

    [Fact]
    public async Task Update_ProductNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.Id = "3";

        // Act
        var result = await productService.Update(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PRODUCT_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Update_SubcategoryNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.SubcategoryId = "3";

        // Act
        var result = await productService.Update(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_SUBCATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Update_AuthorNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.AuthorId = "3";

        // Act
        var result = await productService.Update(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_AUTHOR_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Update_PublisherNotFound_ReturnError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        var productDTO = GenerateSampleProductDTO();
        productDTO.PublisherId = "3";

        // Act
        var result = await productService.Update(productDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PUBLISHER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ReturnTrue()
    {
        // Arrange
        var productId = "1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.Remove(productId);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_ProductNotFound_ReturnError()
    {
        // Arrange
        var productId = "3";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var productService = new ProductService(mockDbContextFactory.Object);

        // Act
        var result = await productService.Remove(productId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PRODUCT_NOT_FOUND, result.Error);
    }
}