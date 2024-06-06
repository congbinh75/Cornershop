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
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = "123";
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
                    req.RequestUri == new Uri("https://example.com/api/user/123")),
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
    public async Task GetById_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "123";

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/user/123")),
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
    public async Task GetCurrentUser_ShouldReturnUser_WhenTokenIsValid()
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
    public async Task GetCurrentUser_ShouldReturnNull_WhenTokenIsInvalid()
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
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
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
    public async Task Login_ShouldReturnNull_WhenCredentialsAreInvalid()
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
    public async Task Register_ShouldReturnUser_WhenRegistrationIsSuccessful()
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
    public async Task Register_ShouldReturnNull_WhenRegistrationFails()
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
    public async Task Update_ShouldReturnUser_WhenUpdateIsSuccessful()
    {
        // Arrange
        var userDto = new UserDTO { FirstName = "John", LastName = "Doe" };
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
    public async Task Update_ShouldReturnNull_WhenUpdateFails()
    {
        // Arrange
        var userDto = new UserDTO { FirstName = "John", LastName = "Doe" };

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
