using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Cornershop.Tests.Service.Domain;

public class SubcategoryServiceTests
{
    #region TestData
    private readonly List<Subcategory> subcategories =
    [
        new()
        {
            Id = "1",
            Name = "Name",
            Description = "Description",
            Category = new Category
            {
                Id = "Category1",
                Name = "Name",
                Description = "Description"
            }
        },
        new()
        {
            Id = "2",
            Name = "Name",
            Description = "Description",
            Category = new Category
            {
                Id = "Category2",
                Name = "Name",
                Description = "Description"
            }
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
            context.Subcategories.AddRange(subcategories);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static SubcategoryDTO GenerateSampleSubcategoryDTO()
    {
        return new SubcategoryDTO
        {
            Id = "1",
            Name = "Name",
            Description = "Description",
            CategoryId = "Category1"
        };
    }
    #endregion

    [Fact]
    public async Task GetById_SubcategoryExists_ReturnsSubcategory()
    {
        // Arrange
        var publisherId = "1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);

        // Act
        var result = await subcategoryService.GetById(publisherId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(publisherId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_SubcategoryNotFound_ReturnsError()
    {
        // Arrange
        var authorId = "3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);

        // Act
        var result = await subcategoryService.GetById(authorId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_SUBCATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetAll_ReturnsSubcategories()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var categoryId = "1";

        // Act
        var result = await subcategoryService.GetAll(1, 10, categoryId);

        // Assert
        Assert.NotNull(result.subcategories);
        Assert.Equal(subcategories.Where(s => s.Category.Id == categoryId).ToList().Count, result.subcategories.Count);
        Assert.Equal(subcategories.Where(s => s.Category.Id == categoryId).ToList().Count, result.count);
    }

    [Fact]
    public async Task Add_ReturnsSubcategory()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryDTO = GenerateSampleSubcategoryDTO();

        // Act
        var result = await subcategoryService.Add(subcategoryDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(subcategoryDTO.Name, result.Value.Name);
    }
    
    [Fact]
    public async Task Add_CategoryNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryDTO = GenerateSampleSubcategoryDTO();
        subcategoryDTO.CategoryId = "3";

        // Act
        var result = await subcategoryService.Add(subcategoryDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Update_ReturnsSubcategory()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryDTO = GenerateSampleSubcategoryDTO();

        // Act
        var result = await subcategoryService.Update(subcategoryDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(subcategoryDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_SubcategoryNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryDTO = GenerateSampleSubcategoryDTO();
        subcategoryDTO.Id = "3";

        // Act
        var result = await subcategoryService.Update(subcategoryDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_SUBCATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Update_CategoryNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryDTO = GenerateSampleSubcategoryDTO();
        subcategoryDTO.CategoryId = "3";

        // Act
        var result = await subcategoryService.Update(subcategoryDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_CATEGORY_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ReturnsTrue()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryId = "1";

        // Act
        var result = await subcategoryService.Remove(subcategoryId);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_SubcategoryNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var subcategoryService = new SubcategoryService(mockDbContextFactory.Object);
        var subcategoryId = "3";

        // Act
        var result = await subcategoryService.Remove(subcategoryId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_SUBCATEGORY_NOT_FOUND, result.Error);
    }
}