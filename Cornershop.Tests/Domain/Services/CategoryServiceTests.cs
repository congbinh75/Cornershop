using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Cornershop.Tests.Service.Domain;

public class CategoryServiceTests
{
    #region TestData
    private readonly List<Category> categories =
    [
        new()
        {
            Id = "1",
            Name = "Name",
            Description = "Description"
        },
        new()
        {
            Id = "2",
            Name = "Name",
            Description = "Description"
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
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static CategoryDTO GenerateSampleCategoryDTO()
    {
        return new CategoryDTO
        {
            Id = "1",
            Name = "Name",
            Description = "Description"
        };
    }
    #endregion

    [Fact]
    public async Task GetById_CategoryExists_ReturnsCategory()
    {
        // Arrange
        var categoryId = "1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);

        // Act
        var result = await categoryService.GetById(categoryId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(categoryId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_CategoryNotFound_ReturnsError()
    {
        // Arrange
        var categoryId = "3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);

        // Act
        var result = await categoryService.GetById(categoryId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetAll_ReturnsCategories()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);

        // Act
        var result = await categoryService.GetAll(1, 10);

        // Assert
        Assert.NotNull(result.categories);
        Assert.Equal(categories.Count, result.categories.Count);
        Assert.Equal(categories.Count, result.count);
    }

    [Fact]
    public async Task Add_ReturnCategory()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);
        var categoryDTO = GenerateSampleCategoryDTO();

        // Act
        var result = await categoryService.Add(categoryDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(categoryDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_ReturnCategory()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);
        var categoryDTO = GenerateSampleCategoryDTO();

        // Act
        var result = await categoryService.Update(categoryDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(categoryDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_CategoryNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);
        var categoryDTO = GenerateSampleCategoryDTO();
        categoryDTO.Id = "3";

        // Act
        var result = await categoryService.Update(categoryDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ReturnsTrue()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);
        var categoryId = "1";

        // Act
        var result = await categoryService.Remove(categoryId);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_CategoryNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var categoryService = new CategoryService(mockDbContextFactory.Object);
        var categoryId = "3";

        // Act
        var result = await categoryService.Remove(categoryId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CATEGORY_NOT_FOUND, result.Error);
    }
}