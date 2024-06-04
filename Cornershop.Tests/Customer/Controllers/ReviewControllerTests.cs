using Cornershop.Presentation.Controllers;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Presentation.Customer.Models;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Cornershop.Tests.Presentation.Customer.Controllers;

public class ReviewControllerTests
{
    private readonly Mock<IReviewService> mockReviewService;
    private readonly ReviewController reviewController;

    public ReviewControllerTests()
    {
        mockReviewService = new Mock<IReviewService>();
        reviewController = new ReviewController(mockReviewService.Object);
    }

    public static IEnumerable<object[]> ProductReviewModelData()
    {
        yield return new object[] {
            new ProductReviewModel
            {
                ProductId = "Test1",
                NewComment = "Comment1",
                NewRating = 1
            }};
        yield return new object[] {
            new ProductReviewModel
            {
                ProductId = "Test2",
                NewComment = "Comment2",
                NewRating = 2
            }};
        yield return new object[] {
            new ProductReviewModel
            {
                ProductId = "Test3",
                NewComment = "Comment3",
                NewRating = 3
            }};
        yield return new object[] {
            new ProductReviewModel
            {
                ProductId = "Test4",
                NewComment = "Comment4",
                NewRating = 4
            }};
        yield return new object[] {
            new ProductReviewModel
            {
                ProductId = "Test5",
                NewComment = "Comment5",
                NewRating = 5
            }};
    }

    [Theory]
    [MemberData(nameof(ProductReviewModelData))]
    public async Task Submit_ShouldCallAddAndRedirectToDetail(ProductReviewModel productReviewModel)
    {
        // Arrange
        var review = new ReviewDTO
        {
            ProductId = productReviewModel.ProductId,
            Comment = productReviewModel.NewComment,
            UserId = "TestUser",
            Rating = productReviewModel.NewRating
        };
        mockReviewService.Setup(service => service.Add(productReviewModel.ProductId, productReviewModel.NewRating,
            productReviewModel.NewComment)).ReturnsAsync(review);

        // Act
        var result = await reviewController.Submit(productReviewModel);

        // Assert
        mockReviewService.Verify(s => s.Add(productReviewModel.ProductId, productReviewModel.NewRating, productReviewModel.NewComment), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
        var redirectResult = result as RedirectToActionResult;
        Assert.Equal("Detail", redirectResult?.ActionName);
        Assert.Equal("Product", redirectResult?.ControllerName);
        Assert.Equal(productReviewModel.ProductId, redirectResult?.RouteValues?["id"]);
    }
}