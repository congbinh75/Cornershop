using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Cornershop.Tests.Service.Domain;

public class PublisherServiceTests
{
    #region TestData
    private readonly List<Publisher> publishers =
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
            context.Publishers.AddRange(publishers);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }
    private static PublisherDTO GenerateSamplePublisherDTO()
    {
        return new PublisherDTO
        {
            Id = "1",
            Name = "Name",
            Description = "Description"
        };
    }
    #endregion

    [Fact]
    public async Task GetById_PublisherExists_ReturnsPublisher()
    {
        // Arrange
        var publisherId = "1";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);

        // Act
        var result = await publisherService.GetById(publisherId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(publisherId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_PublisherNotFound_ReturnsError()
    {
        // Arrange
        var authorId = "3";
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);

        // Act
        var result = await publisherService.GetById(authorId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PUBLISHER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetAll_ReturnsPublishers()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);

        // Act
        var result = await publisherService.GetAll(1, 10);

        // Assert
        Assert.NotNull(result.publishers);
        Assert.Equal(publishers.Count, result.publishers.Count);
        Assert.Equal(publishers.Count, result.count);
    }

    [Fact]
    public async Task Add_ReturnsPublisher()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);
        var publisherDTO = GenerateSamplePublisherDTO();

        // Act
        var result = await publisherService.Add(publisherDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(publisherDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_ReturnsPublisher()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);
        var publisherDTO = GenerateSamplePublisherDTO();

        // Act
        var result = await publisherService.Update(publisherDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(publisherDTO.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_PublisherNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);
        var publisherDTO = GenerateSamplePublisherDTO();
        publisherDTO.Id = "3";

        // Act
        var result = await publisherService.Update(publisherDTO);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PUBLISHER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Remove_ReturnsTrue()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);
        var publisherId = "1";

        // Act
        var result = await publisherService.Remove(publisherId);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_AuthorNotFound_ReturnsError()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var publisherService = new PublisherService(mockDbContextFactory.Object);
        var publisherId = "3";

        // Act
        var result = await publisherService.Remove(publisherId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_PUBLISHER_NOT_FOUND, result.Error);
    }
}