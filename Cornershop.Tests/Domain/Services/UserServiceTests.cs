using System.Diagnostics;
using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Cornershop.Tests.Service.Domain;

public class UserServiceTests
{
    private Mock<ITokenInfoProvider> CreateMockTokenInfoProvider(string userId)
    {
        var mockTokenInfoProvider = new Mock<ITokenInfoProvider>();
        mockTokenInfoProvider.Setup(t => t.Id).Returns(userId);
        return mockTokenInfoProvider;
    }

    [Fact]
    public async Task GetById_UserExists_ReturnsUser()
    {
        // Arrange
        //var options = CreateInMemoryDatabaseOptions();
        var mockTokenInfoProvider = CreateMockTokenInfoProvider("TestUser");

        var userId = "1";
        var users = new List<User>
            {
                new User
                {
                    Id = userId,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Username = "Username1",
                    Email = "user1@example.com",
                    IsEmailConfirmed = false,
                    Role = 2,
                    Password = "HashedPassword",
                    Salt = new byte[] {0},
                    IsBanned = false
                },
                new User
                {
                    Id = "2",
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Username = "Username2",
                    Email = "user2@example.com",
                    IsEmailConfirmed = false,
                    Role = 2,
                    Password = "HashedPassword",
                    Salt = new byte[] {0},
                    IsBanned = false
                }
            };

        var mockDbFactory = new Mock<IDbContextFactory<CornershopDbContext>>();

        var options = new DbContextOptionsBuilder<CornershopDbContext>()
                            .UseInMemoryDatabase(databaseName: "SomeDatabaseInMemory")
                            .Options;

        using (var context = new CornershopDbContext(options, mockTokenInfoProvider.Object))
        {
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

        mockDbFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => new CornershopDbContext(options , mockTokenInfoProvider.Object));

        var userService = new UserService(mockDbFactory.Object);

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.Id);
    }

    // [Fact]
    // public async Task GetById_UserDoesNotExist_ReturnsError()
    // {
    //     // Arrange
    //     var userId = "1";
    //     mockDbContext.Setup(db => db.Users.FindAsync(userId)).ReturnsAsync((User)null);

    //     var userService = new UserService(mockDbContextFactory.Object);

    //     // Act
    //     var result = await userService.GetById(userId);

    //     // Assert
    //     Assert.Null(result.Value);
    //     Assert.Equal(Constants.ERR_USER_NOT_FOUND, result.Error);
    // }

    // [Fact]
    // public async Task GetAll_ReturnsUsers()
    // {
    //     // Arrange
    //     var users = new List<User>
    //     {
    //         new() {
    //             Id = "1",
    //             FirstName = "FirstName",
    //             LastName = "LastName",
    //             Username = "Username",
    //             Email = "johndoe@example.com",
    //             IsEmailConfirmed = false,
    //             Role = 2,
    //             Password = "HashedPassword",
    //             Salt = [0],
    //             IsBanned = false
    //         },
    //         new() {
    //             Id = "2",
    //             FirstName = "FirstName",
    //             LastName = "LastName",
    //             Username = "Username",
    //             Email = "johndoe@example.com",
    //             IsEmailConfirmed = false,
    //             Role = 2,
    //             Password = "HashedPassword",
    //             Salt = [0],
    //             IsBanned = false
    //         },
    //     };
    //     var queryableUsers = users.AsQueryable().BuildMockDbSet();

    //     mockDbContext.Setup(db => db.Users).Returns(queryableUsers.Object);

    //     var userService = new UserService(mockDbContextFactory.Object);

    //     // Act
    //     var result = await userService.GetAll(1, 10, true);

    //     // Assert
    //     Assert.NotEmpty(result.users);
    //     Assert.Equal(users.Count, result.users.Count);
    // }

    // [Fact]
    // public async Task GetByCredentials_ValidCredentials_ReturnsUser()
    // {
    //     // Arrange
    //     var email = "test@example.com";
    //     var password = "password";
    //     var salt = UserService.HashPassword(password).salt;
    //     var hashedPassword = UserService.HashPassword(password, salt).hashedPassword;
    //     var user = new User
    //     {
    //         Id = "1",
    //         FirstName = "FirstName",
    //         LastName = "LastName",
    //         Username = "Username",
    //         Email = "johndoe@example.com",
    //         IsEmailConfirmed = false,
    //         Role = 2,
    //         Password = "HashedPassword",
    //         Salt = [0],
    //         IsBanned = false
    //     };

    //     mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Email == email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

    //     var userService = new UserService(mockDbContextFactory.Object);

    //     // Act
    //     var result = await userService.GetByCredentials(email, password);

    //     // Assert
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(email, result.Value.Email);
    // }

    // [Fact]
    // public async Task Add_UserWithExistingEmail_ReturnsError()
    // {
    //     // Arrange
    //     var userDTO = new UserDTO { Email = "test@example.com", Username = "testuser" };
    //     var existingUser = new User
    //     {
    //         Id = "1",
    //         FirstName = "FirstName",
    //         LastName = "LastName",
    //         Username = "Username",
    //         Email = "johndoe@example.com",
    //         IsEmailConfirmed = false,
    //         Role = 2,
    //         Password = "HashedPassword",
    //         Salt = [0],
    //         IsBanned = false
    //     };

    //     mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

    //     var userService = new UserService(mockDbContextFactory.Object);

    //     // Act
    //     var result = await userService.Add(userDTO);

    //     // Assert
    //     Assert.Null(result.Value);
    //     Assert.Equal(Constants.ERR_EMAIL_ALREADY_REGISTERED, result.Error);
    // }

    // [Fact]
    // public async Task UpdatePassword_ValidOldPassword_UpdatesPassword()
    // {
    //     // Arrange
    //     var userId = "1";
    //     var oldPassword = "oldpassword";
    //     var newPassword = "newpassword";
    //     var salt = UserService.HashPassword(oldPassword).salt;
    //     var hashedOldPassword = UserService.HashPassword(oldPassword, salt).hashedPassword;
    //     var user = new User
    //     {
    //         Id = "1",
    //         FirstName = "FirstName",
    //         LastName = "LastName",
    //         Username = "Username",
    //         Email = "johndoe@example.com",
    //         IsEmailConfirmed = false,
    //         Role = 2,
    //         Password = "HashedPassword",
    //         Salt = [0],
    //         IsBanned = false
    //     };

    //     mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Id == userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

    //     var userService = new UserService(mockDbContextFactory.Object);

    //     // Act
    //     var result = await userService.UpdatePassword(userId, oldPassword, newPassword);

    //     // Assert
    //     Assert.True(result.Value);
    // }

    // [Fact]
    // public async Task UpdatePassword_InvalidOldPassword_ReturnsFalse()
    // {
    //     // Arrange
    //     var userId = "1";
    //     var oldPassword = "wrongpassword";
    //     var newPassword = "newpassword";
    //     var salt = UserService.HashPassword("oldpassword").salt;
    //     var hashedOldPassword = UserService.HashPassword("oldpassword", salt).hashedPassword;
    //     var user = new User
    //     {
    //         Id = "1",
    //         FirstName = "FirstName",
    //         LastName = "LastName",
    //         Username = "Username",
    //         Email = "johndoe@example.com",
    //         IsEmailConfirmed = false,
    //         Role = 2,
    //         Password = "HashedPassword",
    //         Salt = [0],
    //         IsBanned = false
    //     };

    //     mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Id == userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

    //     var userService = new UserService(mockDbContextFactory.Object);

    //     // Act
    //     var result = await userService.UpdatePassword(userId, oldPassword, newPassword);

    //     // Assert
    //     Assert.False(result.Value);
    // }
}