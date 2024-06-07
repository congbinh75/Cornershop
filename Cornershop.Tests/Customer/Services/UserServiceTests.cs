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

public class UserServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly UserService userService;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

    public UserServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockConfiguration = new Mock<IConfiguration>();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockConfiguration.Setup(c => c["Service:BaseAddress"]).Returns("https://example.com/");

        userService = new UserService(mockHttpClientFactory.Object, mockHttpContextAccessor.Object, mockConfiguration.Object);
    }

    [Fact]
    public async Task GetById_ReturnsUser()
    {
        // Arrange
        var userId = "1";
        var expectedUser = new UserDTO { Id = userId, Email = "test@example.com" };
        var response = new GetCurrentUserResponse { User = expectedUser };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/user/" + userId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.Email, result.Email);
    }

    [Fact]
    public async Task GetById_UserNotFound_ReturnsNull()
    {
        // Arrange
        var userId = "1";

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/user/" + userId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCurrentUser_ValidToken_ReturnsUser()
    {
        // Arrange
        var token = "valid-token";
        var expectedUser = new UserDTO { Id = "123", Email = "test@example.com" };
        var response = new GetCurrentUserResponse { User = expectedUser };
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
                    req.RequestUri == new Uri("https://example.com/api/user/current")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await userService.GetCurrentUser();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.Email, result.Email);
    }

    [Fact]
    public async Task GetCurrentUser_InvalidToken_ReturnNull()
    {
        // Arrange
        var token = "invalid-token";

        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/user/current")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        // Act
        var result = await userService.GetCurrentUser();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var userDto = new UserDTO { Email = "test@example.com", PlainPassword = "password" };
        var response = new LoginUserResponse { Token = "valid-token" };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://example.com/api/user/login")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await userService.Login(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.Token, result);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnNull()
    {
        // Arrange
        var userDto = new UserDTO { Email = "test@example.com", PlainPassword = "wrongpassword" };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://example.com/api/user/login")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        // Act
        var result = await userService.Login(userDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Register_ReturnsUser()
    {
        // Arrange
        var userDto = new UserDTO { FirstName = "John", LastName = "Doe", Email = "test@example.com", Username = "johndoe", PlainPassword = "password" };
        var response = new RegisterUserResponse { User = userDto };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://example.com/api/user")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await userService.Register(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.Email, result.Email);
        Assert.Equal(userDto.Username, result.Username);
    }

    [Fact]
    public async Task Register_FailedRegistration_ReturnsNull()
    {
        // Arrange
        var userDto = new UserDTO { FirstName = "John", LastName = "Doe", Email = "test@example.com", Username = "johndoe", PlainPassword = "password" };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://example.com/api/user")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await userService.Register(userDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ReturnsUser()
    {
        // Arrange
        var userDto = new UserDTO { FirstName = "FirstName", LastName = "LastName" };
        var response = new UpdateUserResponse { User = userDto };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Patch && req.RequestUri == new Uri("https://example.com/api/user")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await userService.Update(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.FirstName, result.FirstName);
        Assert.Equal(userDto.LastName, result.LastName);
    }

    [Fact]
    public async Task Update_FailedUpdate_ReturnNull()
    {
        // Arrange
        var userDto = new UserDTO { FirstName = "FirstName", LastName = "LastName" };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Patch &&
                    req.RequestUri == new Uri("https://example.com/api/user")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await userService.Update(userDto);

        // Assert
        Assert.Null(result);
    }
}
