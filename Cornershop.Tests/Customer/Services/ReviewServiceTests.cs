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

public class ReviewServiceTests
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly ReviewService reviewService;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

    public ReviewServiceTests()
    {
        mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockConfiguration = new Mock<IConfiguration>();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockConfiguration.Setup(c => c["Service:BaseAddress"]).Returns("https://example.com/");

        reviewService = new ReviewService(mockHttpClientFactory.Object, mockConfiguration.Object, mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task GetAllByProduct_ReturnsReviews()
    {
        // Arrange
        var productId = "Product1";
        var page = 1;
        var pageSize = 10;

        var expectedReviews = new List<ReviewDTO> { new() { User =  new UserDTO { Id = "1", Email = "test@example.com" }, Product = new ProductDTO { Id = productId }} };
        var response = new GetAllReviewByProductResponse { Reviews = expectedReviews, PagesCount = expectedReviews.Count };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/review" + "?page=" + page + "&pageSize=" + pageSize + "&productId=" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await reviewService.GetAllByProduct(productId, page, pageSize);

        // Assert
        Assert.NotNull(result.reviews);
        Assert.Equal(expectedReviews.Count, result.reviews.Count);
        Assert.Equal(expectedReviews.Count, result.count);
    }

    [Fact]
    public async Task GetAllByProduct_ReviewsNotFound_ReturnsEmpty()
    {
        // Arrange
        var productId = "Product1";
        var page = 1;
        var pageSize = 10;

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/review" + "?page=" + page + "&pageSize=" + pageSize + "&productId=" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await reviewService.GetAllByProduct(productId, 1, 10);

        // Assert
        Assert.Empty(result.reviews);
        Assert.Equal(0, result.count);
    }

    [Fact]
    public async Task GetReviewOfProductByCurrentUser_ReturnsReview()
    {
        // Arrange
        var productId = "Product1";
        var userId = "User1";
        var expectedReview =  new ReviewDTO { User =  new UserDTO { Id = userId, Email = "test@example.com" }, Product = new ProductDTO { Id = productId }};
        var response = new GetReviewOfProductByCurrentUserResponse { Review = expectedReview };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var token = "valid-token";
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/review/current-user" + "?productId=" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await reviewService.GetReviewOfProductByCurrentUser(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.User.Id);
        Assert.Equal(productId, result.Product.Id);
    }

    [Fact]
    public async Task GetReviewOfProductByCurrentUser_ReviewNotFound_ReturnsNull()
    {
        // Arrange
        var productId = "Product1";

        var token = "valid-token";
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://example.com/api/review/current-user" + "?productId=" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await reviewService.GetReviewOfProductByCurrentUser(productId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Add_ReturnsReview()
    {
        // Arrange
        var productId = "Product1";
        var rating = 5;
        var comment = "Comment";
        var token = "valid-token";
        var expectedReview = new ReviewDTO { User =  new UserDTO { Id = "1" }, Product = new ProductDTO { Id = "1"} , Rating = rating, Comment = comment };
        var response = new AddReviewResponse { Review = expectedReview };
        var responseJson = JsonSerializer.Serialize(response, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://example.com/api/review")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await reviewService.Add(productId, rating, comment);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedReview.User.Id, result.User.Id);
        Assert.Equal(expectedReview.Product.Id, result.Product.Id);
        Assert.Equal(expectedReview.Rating, result.Rating);
        Assert.Equal(expectedReview.Comment, result.Comment);
    }

    [Fact]
    public async Task Add_FailedAdding_ReturnsNull()
    {
        // Arrange
        var productId = "Product1";
        var rating = 5;
        var comment = "Comment";
        var token = "valid-token";
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://example.com/api/review")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await reviewService.Add(productId, rating, comment);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Remove_ReturnsTrue()
    {
        // Arrange
        var productId = "Product1";
        var token = "valid-token";
        var responseJson = JsonSerializer.Serialize(new RemoveReviewResponse(), options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri == new Uri("https://example.com/api/review" + "?id=" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await reviewService.Remove(productId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Remove_FailedRemoval_ReturnsFalse()
    {
        // Arrange
        var productId = "Product1";
        var token = "valid-token";
        var responseJson = JsonSerializer.Serialize(new RemoveReviewResponse(), options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        mockHttpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies["AuthCookieClient"]).Returns(token);

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri == new Uri("https://example.com/api/review" + "?id=" + productId)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act
        var result = await reviewService.Remove(productId);

        // Assert
        Assert.False(result);
    }
}