using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Cornershop.Service.Common.Enums;

namespace Cornershop.Tests.Service.Domain;

public class UserServiceTests
{
    #region TestData
    private readonly List<User> users =
    [
        new() {
            Id = "1",
            FirstName = "FirstName1",
            LastName = "LastName1",
            Username = "Username1",
            Email = "test1@example.com",
            IsEmailConfirmed = false,
            Role = (int)Role.Admin,
            Password = UserService.HashPassword("Password1", [0]).hashedPassword,
            Salt = [0],
            IsBanned = false
        },
        new() {
            Id = "2",
            FirstName = "FirstName2",
            LastName = "LastName2",
            Username = "Username2",
            Email = "test2@example.com",
            IsEmailConfirmed = false,
            Role = (int)Role.Staff,
            Password = UserService.HashPassword("Password2", [0]).hashedPassword,
            Salt = [0],
            IsBanned = false
        },
        new() {
            Id = "3",
            FirstName = "FirstName",
            LastName = "LastName",
            Username = "Username",
            Email = "johndoe@example.com",
            IsEmailConfirmed = false,
            Role = (int)Role.Customer,
            Password = UserService.HashPassword("Password3", [0]).hashedPassword,
            Salt = [0],
            IsBanned = false
        },
        new() {
            Id = "4",
            FirstName = "FirstName",
            LastName = "LastName",
            Username = "Username",
            Email = "johndoe@example.com",
            IsEmailConfirmed = false,
            Role = (int)Role.Customer,
            Password = UserService.HashPassword("Password4", [0]).hashedPassword,
            Salt = [0],
            IsBanned = false
        },
    ];
    #endregion

    private async Task<Mock<IDbContextFactory<CornershopDbContext>>> CreateMockDbContextFactoryAsync()
    {
        var mockTokenInfoProvider = new Mock<ITokenInfoProvider>();
        mockTokenInfoProvider.Setup(t => t.Id).Returns("TestUser");

        var options = new DbContextOptionsBuilder<CornershopDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;

        using (var context = new CornershopDbContext(options, mockTokenInfoProvider.Object))
        {
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }
        var mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CornershopDbContext(options, mockTokenInfoProvider.Object));

        return mockDbContextFactory;
    }

    [Fact]
    public async Task GetById_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = "1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.Id);
    }

    [Fact]
    public async Task GetById_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        var userId = "5";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(Constants.ERR_USER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task GetAll_ReturnsUsers()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetAll(1, 10, false);

        // Assert
        Assert.NotEmpty(result.users);
        Assert.Equal(users.Count, result.users.Count);
    }

    [Fact]
    public async Task GetAllCustomersOnly_ReturnsUsers()
    {
        // Arrange
        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetAll(1, 10, true);

        // Assert
        Assert.NotEmpty(result.users);
        Assert.Equal(users.Where(c => c.Role == (int)Role.Customer).ToList().Count, result.users.Count);
    }

    [Fact]
    public async Task GetByCredentials_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var email = "test1@example.com";
        var password = "Password1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetByCredentials(email, password);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(email, result.Value.Email);
    }

    [Fact]
    public async Task GetByCredentials_InvalidCredentials_ReturnsError()
    {
        // Arrange
        var email = "wrongtest1@example.com";
        var password = "WrongPassword1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetByCredentials(email, password);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(Constants.ERR_USER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task Add_ReturnsUser()
    {
        // Arrange
        var email = "test@example.com";
        var userDTO = new UserDTO
        {
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            Username = "NEwUsername",
            Email = email,
            PlainPassword = "NewPassword"
        };

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Add(userDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(email, result.Value.Email);
    }

    [Fact]
    public async Task Add_UserWithExistingEmail_ReturnsError()
    {
        // Arrange
        var userDTO = new UserDTO { Email = "test1@example.com", Username = "testuser" };

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Add(userDTO);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(Constants.ERR_EMAIL_ALREADY_REGISTERED, result.Error);
    }

    [Fact]
    public async Task Add_UserWithExistingUsername_ReturnsError()
    {
        // Arrange
        var userDTO = new UserDTO { Email = "test@example.com", Username = "Username1" };

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Add(userDTO);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(Constants.ERR_USERNAME_ALREADY_REGISTERED, result.Error);
    }

    [Fact]
    public async Task Update_ReturnUser()
    {
        // Arrange
        var userDTO = new UserDTO
        {
            Id = "1",
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            Role = (int)Role.Customer,
            IsBanned = true
        };

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Update(userDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(userDTO.FirstName, result.Value.FirstName);
        Assert.Equal(userDTO.LastName, result.Value.LastName);
        Assert.Equal(userDTO.Role, result.Value.Role);
        Assert.Equal(userDTO.IsBanned, result.Value.IsBanned);
    }

    [Fact]
    public async Task Update_UserNotFound_ReturnError()
    {
        // Arrange
        var userDTO = new UserDTO
        {
            Id = "5",
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            Role = (int)Role.Customer,
            IsBanned = true
        };

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Update(userDTO);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(Constants.ERR_USER_NOT_FOUND, result.Error);
    }

    [Fact]
    public async Task UpdatePassword_ValidOldPassword_UpdatesPassword()
    {
        // Arrange
        var userId = "1";
        var oldPassword = "Password1";
        var newPassword = "NewPassword1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.UpdatePassword(userId, oldPassword, newPassword);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task UpdatePassword_InvalidOldPassword_ReturnsFalse()
    {
        // Arrange
        var userId = "1";
        var oldPassword = "WrongPassword1";
        var newPassword = "NewPassword1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.UpdatePassword(userId, oldPassword, newPassword);

        // Assert
        Assert.False(result.Value);
    }

    [Fact]
    public async Task Remove_ReturnTrue()
    {
        // Arrange
        var userId = "1";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Remove(userId);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Remove_UserNotFound_ReturnError()
    {
        // Arrange
        var userId = "5";

        var mockDbContextFactory = await CreateMockDbContextFactoryAsync();
        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Remove(userId);

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(Constants.ERR_USER_NOT_FOUND, result.Error);
    }
}