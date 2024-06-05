using Cornershop.Service.Common;
using Cornershop.Service.Domain.Services;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Cornershop.Tests.Service.Domain;

public class UserServiceTests
{
    private Mock<IDbContextFactory<CornershopDbContext>> mockDbContextFactory;
    private Mock<CornershopDbContext> mockDbContext;
    private Mock<DbSet<User>> mockUserDbSet;

    public UserServiceTests()
    {
        mockDbContextFactory = new Mock<IDbContextFactory<CornershopDbContext>>();
        mockDbContext = new Mock<CornershopDbContext>(new DbContextOptions<CornershopDbContext>());
        mockUserDbSet = CreateMockDbSet(new List<User>());

        mockDbContextFactory.Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => mockDbContext.Object);
        mockDbContext.Setup(db => db.Users).Returns(mockUserDbSet.Object);
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> elements) where T : class
    {
        var queryable = elements.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        dbSetMock.Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>())).Callback<T, CancellationToken>((s, c) => elements.Add(s)).ReturnsAsync((T s, CancellationToken c) => new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T>(new Mock<Microsoft.EntityFrameworkCore.Metadata.Internal.InternalEntityEntry>().Object));
        dbSetMock.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(s => elements.Remove(s));

        return dbSetMock;
    }

    [Fact]
    public async Task GetById_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = "1";
        var users = new List<User>
        {
            new() {
                Id = userId,
                FirstName = "FirstName",
                LastName = "LastName",
                Username = "Username",
                Email = "johndoe@example.com",
                IsEmailConfirmed = false,
                Role = 2,
                Password = "HashedPassword",
                Salt = [0],
                IsBanned = false
            }
        };
        mockUserDbSet = CreateMockDbSet(users);

        mockDbContext.Setup(db => db.Users).Returns(mockDbContext.Object);

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
        var userId = "1";
        mockDbContext.Setup(db => db.Users.FindAsync(userId)).ReturnsAsync((User)null);

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
        var users = new List<User>
        {
            new() {
                Id = "1",
                FirstName = "FirstName",
                LastName = "LastName",
                Username = "Username",
                Email = "johndoe@example.com",
                IsEmailConfirmed = false,
                Role = 2,
                Password = "HashedPassword",
                Salt = [0],
                IsBanned = false
            },
            new() {
                Id = "2",
                FirstName = "FirstName",
                LastName = "LastName",
                Username = "Username",
                Email = "johndoe@example.com",
                IsEmailConfirmed = false,
                Role = 2,
                Password = "HashedPassword",
                Salt = [0],
                IsBanned = false
            },
        };
        var queryableUsers = users.AsQueryable().BuildMockDbSet();

        mockDbContext.Setup(db => db.Users).Returns(queryableUsers.Object);

        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetAll(1, 10, true);

        // Assert
        Assert.NotEmpty(result.users);
        Assert.Equal(users.Count, result.users.Count);
    }

    [Fact]
    public async Task GetByCredentials_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password";
        var salt = UserService.HashPassword(password).salt;
        var hashedPassword = UserService.HashPassword(password, salt).hashedPassword;
        var user = new User
        {
            Id = "1",
            FirstName = "FirstName",
            LastName = "LastName",
            Username = "Username",
            Email = "johndoe@example.com",
            IsEmailConfirmed = false,
            Role = 2,
            Password = "HashedPassword",
            Salt = [0],
            IsBanned = false
        };

        mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Email == email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.GetByCredentials(email, password);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(email, result.Value.Email);
    }

    [Fact]
    public async Task Add_UserWithExistingEmail_ReturnsError()
    {
        // Arrange
        var userDTO = new UserDTO { Email = "test@example.com", Username = "testuser" };
        var existingUser = new User
        {
            Id = "1",
            FirstName = "FirstName",
            LastName = "LastName",
            Username = "Username",
            Email = "johndoe@example.com",
            IsEmailConfirmed = false,
            Role = 2,
            Password = "HashedPassword",
            Salt = [0],
            IsBanned = false
        };

        mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.Add(userDTO);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(Constants.ERR_EMAIL_ALREADY_REGISTERED, result.Error);
    }

    [Fact]
    public async Task UpdatePassword_ValidOldPassword_UpdatesPassword()
    {
        // Arrange
        var userId = "1";
        var oldPassword = "oldpassword";
        var newPassword = "newpassword";
        var salt = UserService.HashPassword(oldPassword).salt;
        var hashedOldPassword = UserService.HashPassword(oldPassword, salt).hashedPassword;
        var user = new User
        {
            Id = "1",
            FirstName = "FirstName",
            LastName = "LastName",
            Username = "Username",
            Email = "johndoe@example.com",
            IsEmailConfirmed = false,
            Role = 2,
            Password = "HashedPassword",
            Salt = [0],
            IsBanned = false
        };

        mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Id == userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

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
        var oldPassword = "wrongpassword";
        var newPassword = "newpassword";
        var salt = UserService.HashPassword("oldpassword").salt;
        var hashedOldPassword = UserService.HashPassword("oldpassword", salt).hashedPassword;
        var user = new User
        {
            Id = "1",
            FirstName = "FirstName",
            LastName = "LastName",
            Username = "Username",
            Email = "johndoe@example.com",
            IsEmailConfirmed = false,
            Role = 2,
            Password = "HashedPassword",
            Salt = [0],
            IsBanned = false
        };

        mockDbContext.Setup(db => db.Users.FirstOrDefaultAsync(u => u.Id == userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var userService = new UserService(mockDbContextFactory.Object);

        // Act
        var result = await userService.UpdatePassword(userId, oldPassword, newPassword);

        // Assert
        Assert.False(result.Value);
    }
}