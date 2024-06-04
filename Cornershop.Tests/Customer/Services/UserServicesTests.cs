using System.Net;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Controllers;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Models;
using Cornershop.Presentation.Customer.Services;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Cornershop.Tests.Presentation.Customer.Services;

public class UserServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly UserService userService;

    public UserServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var configForBaseAddress = new Dictionary<string, string>
        {
            {"Service:BaseAddress", "http://localhost:7000"},
        };
        
       var mockConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForBaseAddress)
            .Build();
        
        userService = new UserService(mockHttpClientFactory.Object, mockHttpContextAccessor.Object, mockConfiguration);
    }

    [Fact]
    public async Task GetById_ReturnsUserDTO_WhenSuccessStatusCode()
    {
        // Arrange
        var userId = "123";
        var userDTO = new UserDTO { Id = userId, FirstName = "John", LastName = "Doe" };
        var responseContent = new GetCurrentUserResponse { User = userDTO };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var userId = "123";
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCurrentUser_ReturnsUserDTO_WhenSuccessStatusCode()
    {
        // Arrange
        var userDTO = new UserDTO { Id = "123", FirstName = "John", LastName = "Doe" };
        var responseContent = new GetCurrentUserResponse { User = userDTO };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request.Cookies["AuthCookieClient"]).Returns("mock-token");
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        // Act
        var result = await userService.GetCurrentUser();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDTO.Id, result.Id);
    }

    [Fact]
    public async Task GetCurrentUser_ReturnsNull_WhenNoToken()
    {
        // Arrange
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

        // Act
        var result = await userService.GetCurrentUser();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenSuccessStatusCode()
    {
        // Arrange
        var userDTO = new UserDTO { Email = "john.doe@example.com", PlainPassword = "password123" };
        var responseContent = new LoginUserResponse { Token = "mock-token" };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.Login(userDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("mock-token", result);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenUnauthorized()
    {
        // Arrange
        var userDTO = new UserDTO { Email = "john.doe@example.com", PlainPassword = "password123" };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Unauthorized
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.Login(userDTO);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Register_ReturnsUserDTO_WhenSuccessStatusCode()
    {
        // Arrange
        var userDTO = new UserDTO { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Username = "johndoe", PlainPassword = "password123" };
        var responseContent = new RegisterUserResponse { User = userDTO };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.Register(userDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDTO.Email, result.Email);
    }

    [Fact]
    public async Task Register_ReturnsNull_WhenConflict()
    {
        // Arrange
        var userDTO = new UserDTO { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Username = "johndoe", PlainPassword = "password123" };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.Register(userDTO);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ReturnsUserDTO_WhenSuccessStatusCode()
    {
        // Arrange
        var userDTO = new UserDTO { Id = "123", FirstName = "John", LastName = "Doe" };
        var responseContent = new UpdateUserResponse { User = userDTO };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.PatchAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.Update(userDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDTO.Id, result.Id);
    }

    [Fact]
    public async Task Update_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var userDTO = new UserDTO { Id = "123", FirstName = "John", LastName = "Doe" };
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        mockHttpClient.Setup(c => c.PatchAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(httpResponseMessage);

        // Act
        var result = await userService.Update(userDTO);

        // Assert
        Assert.Null(result);
    }
}