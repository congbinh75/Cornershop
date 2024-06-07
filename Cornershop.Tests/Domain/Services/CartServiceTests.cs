using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Cornershop.Service.Common.Enums;

namespace Cornershop.Tests.Service.Domain;

public class CartServiceTests
{
    #region TestData
    private readonly List<Cart> carts =
    [
        new()
        {
            User = new User
            {
                Id = "User1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Username = "Username1",
                Email = "test1@example.com",
                IsEmailConfirmed = false,
                Role = (int)Role.Admin,
                Password = UserService.HashPassword("Password1", [0]).hashedPassword,
                Salt = [0],
                IsBanned = false
            },
            UserId = "User1",
            CartDetails = []
        },
        new()
        {
            User = new User
            {
                Id = "User2",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Username = "Username1",
                Email = "test1@example.com",
                IsEmailConfirmed = false,
                Role = (int)Role.Admin,
                Password = UserService.HashPassword("Password1", [0]).hashedPassword,
                Salt = [0],
                IsBanned = false
            },
           UserId = "User2",
           CartDetails = []
        }
    ];
    private readonly List<Product> products = 
    [
        new Product
        {
            Id = "Product1",
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
            context.Carts.AddRange(carts);
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static CartDetailDTO GenerateSampleCartDetailDTO()
    {
        return new CartDetailDTO
        {
            ProductId = "Product3",
            CartId = "User1",
            Quantity = 1
        };
    }
    #endregion

    [Fact]
    public async Task GetByUserId_ReturnsCart()
    {
        // Arrange
        var userId = "User1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.GetByUserId(userId);

        // Assert
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetByUserId_CartNotFound_ReturnsError()
    {
        // Arrange
        var cartId = "Cart3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.GetByUserId(cartId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CART_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task AddItem_ReturnCart()
    {
        // Arrange
        var userId = "User1";
        var productId = "Product1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.AddItem(userId, productId, 1);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.CartDetails);
    }

    [Fact]
    public async Task AddItem_CartNotFound_ReturnError()
    {
        // Arrange
        var userId = "User3";
        var productId = "Product1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.AddItem(userId, productId, 1);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CART_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task AddItem_ProductNotFound_ReturnError()
    {
        // Arrange
        var userId = "User1";
        var productId = "Product3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.AddItem(userId, productId, 1);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PRODUCT_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task RemoveItem_CartNotFound_ReturnError()
    {
        // Arrange
        var userId = "User3";
        var productId = "Product1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.RemoveItem(userId, productId, 1);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CART_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task RemoveItem_CartDetailNotFound_ReturnError()
    {
        // Arrange
        var userId = "User1";
        var productId = "Product3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var cartService = new CartService(mockDbContextFactory.Object);

        // Act
        var result = await cartService.RemoveItem(userId, productId, 1);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CART_DETAIL_NOT_FOUND, result.Error);
    }
}