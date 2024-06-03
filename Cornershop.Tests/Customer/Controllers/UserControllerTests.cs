using Cornershop.Presentation.Controllers;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Cornershop.Tests.Presentation.Customer.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> mockUserService;
    private readonly UserController userController;
    private readonly Mock<IResponseCookies> mockCookies;
    private readonly Mock<HttpContext> mockHttpContext;

    public UserControllerTests()
    {
        mockUserService = new Mock<IUserService>();
        userController = new UserController(mockUserService.Object);
        mockCookies = new Mock<IResponseCookies>();
        mockHttpContext = new Mock<HttpContext>();
    }

    [Fact]
    public async Task Detail_ReturnsViewResult_WithUserDTO_WhenIdIs1()
    {
        // Arrange
        var userId = "1";
        var user = new UserDTO { Id = userId };
        mockUserService.Setup(service => service.GetById(userId)).ReturnsAsync(user);

        // Act
        var result = await userController.Detail(userId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<UserDTO>(viewResult.ViewData.Model);
        Assert.Equal(userId, model.Id);
    }

    [Fact]
    public async Task Detail_ReturnsNullViewModelResult_WhenIdIs2()
    {
        // Arrange
        var userId = "2";
        mockUserService.Setup(service => service.GetById(userId)).ReturnsAsync((UserDTO?)null);

        // Act
        var result = await userController.Detail(userId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
    }

    [Fact]
    public void Logout_RemovesAuthCookie_AndRedirectsToHomeIndex()
    {
        //Arrange
        mockHttpContext.Setup(ctx => ctx.Response.Cookies).Returns(mockCookies.Object);
        userController.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };

        // Act
        var result = userController.Logout();

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        var redirectResult = result as RedirectToActionResult;
        Assert.Equal("Index", redirectResult?.ActionName);
        Assert.Equal("Home", redirectResult?.ControllerName);
    }
}