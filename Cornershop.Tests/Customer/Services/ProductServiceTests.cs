using System.Net;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Services;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace Cornershop.Tests.Presentation.Customer;

public class ProductServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly ProductService productService;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

    public ProductServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockConfiguration = new Mock<IConfiguration>();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockConfiguration.Setup(c => c["Service:BaseAddress"]).Returns("https://example.com/");

        productService = new ProductService(mockHttpClientFactory.Object, mockConfiguration.Object);
    }

    [Fact]
    public async Task GetById_ReturnsProduct()
    {
        // Arrange
        var productId = "Product1";
        var expectedProduct = new ProductDTO { Id = productId, Name = "Name" };
        var response = new GetProductResponse { Product = expectedProduct };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/product/" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await productService.GetById(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
    }

    [Fact]
    public async Task GetById_ProductNotFound_ReturnsNull()
    {
        // Arrange
        var productId = "Product1";

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/product/" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await productService.GetById(productId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsProducts()
    {
        // Arrange
        var expectedProducts = new List<ProductDTO> { new() { Id = "1", Name = "Name" } };
        var response = new GetAllProductResponse { Products = expectedProducts, PagesCount = expectedProducts.Count };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var page = 1;
        var pageSize = 10;
        var keyword = "keyword";
        var categoryId = "Category1";
        var subcategoryId = "Subcategory1";

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/product" 
                        + "?page=" + page + "&pageSize=" + pageSize + "&keyword=" + keyword + "&categoryId=" + categoryId + "&subcategoryId=" + subcategoryId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var (products, count) = await productService.GetAll(1, 10, keyword, categoryId, subcategoryId);

        // Assert
        Assert.NotNull(products);
        Assert.Equal(expectedProducts.Count, products.Count);
        Assert.Equal(expectedProducts.Count, count);
    }

    [Fact]
    public async Task GetAll_ProductsNotFound_ReturnsEmpty()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;
        var keyword = "keyword";
        var categoryId = "Category1";
        var subcategoryId = "Subcategory1";

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/product" 
                        + "?page=" + page + "&pageSize=" + pageSize + "&keyword=" + keyword + "&categoryId=" + categoryId + "&subcategoryId=" + subcategoryId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var (products, count) = await productService.GetAll(1, 10, keyword, categoryId, subcategoryId);

        // Assert
        Assert.Empty(products);
        Assert.Equal(0, count);
    }
}