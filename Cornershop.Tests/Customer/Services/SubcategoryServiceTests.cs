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

public class SubcategoryServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly SubcategoryService subcategoryService;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

    public SubcategoryServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockConfiguration = new Mock<IConfiguration>();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockConfiguration.Setup(c => c["Service:BaseAddress"]).Returns("https://example.com/");

        subcategoryService = new SubcategoryService(mockHttpClientFactory.Object, mockConfiguration.Object);
    }

    [Fact]
    public async Task GetAllByCategory_ReturnsCategories()
    {
        // Arrange
        var categoryId = "Category1";
        var expectedSubcategories = new List<SubcategoryDTO> { new() { Id = "1", Name = "Name" } };
        var response = new GetAllSubcategoryResponse { Subcategories = expectedSubcategories, PagesCount = expectedSubcategories.Count };
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
                    req.RequestUri == new Uri("https://example.com/api/subcategory" 
                        + "?page=" + page + "&pageSize=" + pageSize + "&categoryId=" + categoryId )),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var categories = await subcategoryService.GetAllByCategory(categoryId, page, pageSize);

        // Assert
        Assert.NotNull(categories);
        Assert.Equal(expectedSubcategories.Count, categories.Count);
    }

    [Fact]
    public async Task GetAll_ProductsNotFound_ReturnsEmpty()
    {
        // Arrange
        var categoryId = "Category1";
        var page = 1;
        var pageSize = 10;

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/subcategory" 
                        + "?page=" + page + "&pageSize=" + pageSize + "&categoryId=" + categoryId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var subcategories = await subcategoryService.GetAllByCategory(categoryId, page, pageSize);

        // Assert
        Assert.Empty(subcategories);
    }
}