using System.Net;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Services;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace Cornershop.Tests.Presentation.Customer;

public class CategoryServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly CategoryService categoryService;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

    public CategoryServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockConfiguration = new Mock<IConfiguration>();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockConfiguration.Setup(c => c["Service:BaseAddress"]).Returns("https://example.com/");

        categoryService = new CategoryService(mockHttpClientFactory.Object, mockConfiguration.Object);
    }

    [Fact]
    public async Task GetById_ReturnsCategory()
    {
        // Arrange
        var categoryId = "Category1";
        var expectedCategory = new CategoryDTO { Id = categoryId, Name = "Name" };
        var response = new GetCategoryResponse { Category = expectedCategory };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/category/" + categoryId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await categoryService.GetById(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategory.Id, result.Id);
        Assert.Equal(expectedCategory.Name, result.Name);
    }

    [Fact]
    public async Task GetById_CategoryNotFound_ReturnsNull()
    {
        // Arrange
        var categoryId = "Category1";

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/category/" + categoryId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await categoryService.GetById(categoryId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsCategories()
    {
        // Arrange
        var expectedCategories = new List<CategoryDTO> { new() { Id = "1", Name = "Name" } };
        var response = new GetAllCategoryResponse { Categories = expectedCategories, PagesCount = expectedCategories.Count };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var page = 1;
        var pageSize = 10;

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/category" 
                        + "?page=" + page + "&pageSize=" + pageSize)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var categories = await categoryService.GetAll(1, 10);

        // Assert
        Assert.NotNull(categories);
        Assert.Equal(expectedCategories.Count, categories.Count);
    }

    [Fact]
    public async Task GetAll_ProductsNotFound_ReturnsEmpty()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/category" 
                        + "?page=" + page + "&pageSize=" + pageSize)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var categories = await categoryService.GetAll(1, 10);

        // Assert
        Assert.Empty(categories);
    }
}