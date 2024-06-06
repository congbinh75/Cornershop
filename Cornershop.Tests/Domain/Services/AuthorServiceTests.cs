using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Cornershop.Tests.Service.Domain;

public class AuthorServiceTests
{
    #region TestData
    private readonly List<Author> authors =
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
            context.Authors.AddRange(authors);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static AuthorDTO GenerateSampleAuthorDTO()
    {
        return new AuthorDTO
        {
            Id = "1",
            Name = "Name",
            Description = "Description"
        };
    }
    #endregion

    [Fact]
    public async Task GetById_AuthorExists_ReturnsAuthor()
    {
        // Arrange
        var authorId = "1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);

        // Act
        var result = await authorService.GetById(authorId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(authorId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_AuthorNotFound_ReturnsError()
    {
        // Arrange
        var authorId = "3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);

        // Act
        var result = await authorService.GetById(authorId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_AUTHOR_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetAll_ReturnsAuthors()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);

        // Act
        var result = await authorService.GetAll(1, 10);

        // Assert
        Assert.NotNull(result.authors);
        Assert.Equal(authors.Count, result.authors.Count);
        Assert.Equal(authors.Count, result.count);
    }

    [Fact]
    public async Task Add_ReturnsAuthor()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);
        var authorDTO = GenerateSampleAuthorDTO();

        // Act
        var result = await authorService.Add(authorDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(authorDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_ReturnsAuthor()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);
        var authorDTO = GenerateSampleAuthorDTO();

        // Act
        var result = await authorService.Update(authorDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(authorDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_AuthorNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);
        var authorDTO = GenerateSampleAuthorDTO();
        authorDTO.Id = "3";

        // Act
        var result = await authorService.Update(authorDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_AUTHOR_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ReturnsTrue()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);
        var authorId = "1";

        // Act
        var result = await authorService.Remove(authorId);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_AuthorNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var authorService = new AuthorService(mockDbContextFactory.Object);
        var authorId = "3";

        // Act
        var result = await authorService.Remove(authorId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_AUTHOR_NOT_FOUND, result.Error);
    }
}