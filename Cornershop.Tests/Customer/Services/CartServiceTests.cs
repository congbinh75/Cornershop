using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Services;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Cornershop.Tests.Presentation.Customer;

public class CartServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly CartService cartService;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

    public CartServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockConfiguration = new Mock<IConfiguration>();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockConfiguration.Setup(c => c["Service:BaseAddress"]).Returns("https://example.com/");

        cartService = new CartService(mockHttpClientFactory.Object, mockConfiguration.Object, mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task GetCartByCurrentUser_ReturnsCart()
    {
        // Arrange
        var token = "valid-token";
        var expectedCart = new CartDTO { User =  new UserDTO { Id = "123", Email = "test@example.com" }};
        var response = new GetCartByCurrentUserResponse { Cart = expectedCart };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/cart")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await cartService.GetCartByCurrentUser();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCartByCurrentUser_CartNotFound_ReturnsNull()
    {
        // Arrange
        var token = "valid-token";
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/cart")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await cartService.GetCartByCurrentUser();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddItem_ReturnsCart()
    {
        // Arrange
        var productId = "Product1";
        var quantity = 1;
        var token = "valid-token";
        var expectedCart = new CartDTO { User =  new UserDTO { Id = "123", Email = "test@example.com" }};
        var response = new AddItemCartResponse { Cart = expectedCart };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://example.com/api/cart/add")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await cartService.AddItem(productId, quantity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddItem_FailedAdding_ReturnsNull()
    {
        // Arrange
        var productId = "Product1";
        var quantity = 1;
        var token = "valid-token";
        var expectedCart = new CartDTO { User =  new UserDTO { Id = "123", Email = "test@example.com" }};
        var response = new AddItemCartResponse { Cart = expectedCart };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://example.com/api/cart/add")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await cartService.AddItem(productId, quantity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveItem_ReturnsCart()
    {
        // Arrange
        var productId = "Product1";
        var quantity = 1;
        var token = "valid-token";
        var expectedCart = new CartDTO { User =  new UserDTO { Id = "123", Email = "test@example.com" }};
        var response = new RemoveItemCartResponse { Cart = expectedCart };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://example.com/api/cart/remove")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await cartService.RemoveItem(productId, quantity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RemoveItem_FailedRemoval_ReturnsNull()
    {
        // Arrange
        var productId = "Product1";
        var quantity = 1;
        var token = "valid-token";
        var expectedCart = new CartDTO { User =  new UserDTO { Id = "123", Email = "test@example.com" }};
        var response = new RemoveItemCartResponse { Cart = expectedCart };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://example.com/api/cart/remove")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await cartService.RemoveItem(productId, quantity);

        // Assert
        Assert.False(result);
    }
}