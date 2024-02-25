using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }
    [Fact]
    public void Create_SavesUserInDataContext()
    {
        // Arrange
        var service = CreateService();
        var userToCreate = new User { Forename = "New", Surname = "User" };

        // Act
        service.Create(userToCreate);

        // Assert
        _dataContext.Verify(m => m.Create(userToCreate), Times.Once());
    }
    [Fact]
    public void GetAll_ShouldReturnAllUsers()
    {
        // Arrange
        var expectedUsers = new List<User>
        {
            // Set up sample users here
        };

        // Create a mock for the DataContext
        var mockDataContext = new Mock<IDataContext>();
        mockDataContext.Setup(x => x.GetAll<User>()).Returns(expectedUsers.AsQueryable());

        var userService = new UserService(mockDataContext.Object);

        // Act
        var result = userService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(expectedUsers);
    }
    [Fact]
    public void Create_ShouldAddNewUser()
    {
        // Arrange
        var newUser = new User { Forename = "Test", Surname = "User" };
        var mockDataContext = new Mock<IDataContext>();

        var userService = new UserService(mockDataContext.Object);

        // Act
        userService.Create(newUser);

        // Assert
        mockDataContext.Verify(x => x.Create(It.Is<User>(u => u == newUser)), Times.Once());
    }
    [Fact]
    public void FilterByActive_WhenCalledWithTrue_ReturnsOnlyActiveUsers()
    {
        // Arrange
        var activeUsers = new List<User> { /* Set up active users */ };
        var inactiveUsers = new List<User> { /* Set up inactive users */ };
        var mockDataContext = new Mock<IDataContext>();

        // Setup the mock DataContext to return appropriate results based on filters
        mockDataContext.Setup(x => x.GetAll<User>()).Returns(
            activeUsers.Concat(inactiveUsers).AsQueryable()
        );

        var userService = new UserService(mockDataContext.Object);

        // Act
        var result = userService.FilterByActive(true);

        // Assert
        result.Should().BeEquivalentTo(activeUsers);
    }
    [Fact]
    public void GetUserID_WhenUserExists_ReturnsCorrectUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User { Id = userId, /* ... other properties */ };
        var mockDataContext = new Mock<IDataContext>();
        mockDataContext.Setup(x => x.GetAll<User>()).Returns(new List<User> { expectedUser }.AsQueryable());

        var userService = new UserService(mockDataContext.Object);

        // Act
        var result = userService.GetUserID(userId).FirstOrDefault();

        // Assert
        result.Should().Be(expectedUser);
    }
    [Fact]
    public void Update_WhenUserExists_ModifiesTheUser()
    {
        // Arrange
        var userId = 1;
        var existingUser = new User { Id = userId, Forename = "Old Name" };
        var updatedUser = new User { Id = userId, Forename = "New Name" };
        var mockDataContext = new Mock<IDataContext>();
        mockDataContext.Setup(x => x.GetAll<User>()).Returns(new List<User> { existingUser }.AsQueryable());

        var userService = new UserService(mockDataContext.Object);

        // Act
        userService.Update(updatedUser);

        // Assert
        mockDataContext.Verify(x => x.Update(It.Is<User>(u => u.Id == userId && u.Forename == "New Name")), Times.Once());
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
