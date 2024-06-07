using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Cornershop.Service.Common.Enums;

namespace Cornershop.Tests.Service.Domain;

public class ReviewServiceTests
{
    #region TestData
    private readonly List<Review> reviews =
    [
        new()
        {
            Id = "1",
            Product = new Product
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
            },
            Rating = 5,
            Comment = "Comment",
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
            }
        },
        new()
        {
            Id = "2",
            Product = new Product
            {
                Id = "Product2",
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
                SubcategoryId = "Subcategory1",
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
                IsVisible = true
            },
            Rating = 5,
            Comment = "Comment",
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
            }
        },
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
            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static ReviewDTO GenerateSampleReviewDTO()
    {
        return new ReviewDTO
        {
            ProductId = "Product2",
            UserId = "User1",
            Rating = 5,
            Comment = "Comment"
        };
    }
    #endregion

    [Fact]
    public async Task GetAllByProduct_ReturnsReviews()
    {
        // Arrange
        var productId = "Product1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);

        // Act
        var result = await reviewService.GetAllByProduct(1, 10, productId);

        // Assert
        Assert.NotNull(result.reviews);
        Assert.Equal(reviews.Where(r => r.Product.Id == productId).ToList().Count, result.reviews.Count);
        Assert.Equal(reviews.Where(r => r.Product.Id == productId).ToList().Count, result.count);
    }

    [Fact]
    public async Task GetByProductAndUser_ReturnsReview()
    {
        // Arrange
        var productId = "Product1";
        var userId = "User1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);

        // Act
        var result = await reviewService.GetByProductAndUser(productId, userId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.User.Id);
    }

    [Fact]
    public async Task GetByProductAndUser_ReviewNotFound_ReturnsError()
    {
        // Arrange
        var productId = "Product3";
        var userId = "User3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);

        // Act
        var result = await reviewService.GetByProductAndUser(productId, userId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_REVIEW_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Add_ReturnsReview()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = GenerateSampleReviewDTO();

        // Act
        var result = await reviewService.Add(reviewDTO);

        // Assert
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Add_ReviewForProductByUserAlreadyExists_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = GenerateSampleReviewDTO();
        reviewDTO.ProductId = "Product1";
        reviewDTO.UserId = "User1";

        // Act
        var result = await reviewService.Add(reviewDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_REVIEW_FOR_PRODUCT_BY_USER_EXISTED, result.Error);
    }

    [Fact]
    public async Task Add_UserNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = GenerateSampleReviewDTO();
        reviewDTO.ProductId = "Product1";
        reviewDTO.UserId = "User3";

        // Act
        var result = await reviewService.Add(reviewDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_USER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Add_ProductNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = GenerateSampleReviewDTO();
        reviewDTO.ProductId = "Product3";
        reviewDTO.UserId = "User1";

        // Act
        var result = await reviewService.Add(reviewDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PRODUCT_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ReturnsTrue()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = new ReviewDTO
        {
            ProductId = "Product1",
            UserId = "User1",
        };

        // Act
        var result = await reviewService.Remove(reviewDTO);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_UserNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = new ReviewDTO
        {
            ProductId = "Product1",
            UserId = "User3",
        };

        // Act
        var result = await reviewService.Remove(reviewDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_REVIEW_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ProductNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var reviewService = new ReviewService(mockDbContextFactory.Object);
        var reviewDTO = new ReviewDTO
        {
            ProductId = "Product3",
            UserId = "User1",
        };

        // Act
        var result = await reviewService.Remove(reviewDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_REVIEW_NOT_FOUND, result.Error);
    }
}