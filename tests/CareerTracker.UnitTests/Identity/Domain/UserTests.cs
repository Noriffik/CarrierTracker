using CareerTracker.Identity.Domain;
using FluentAssertions;

namespace CareerTracker.UnitTests.Identity.Domain;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ReturnsActiveUser()
    {
        // Act
        var user = User.Create("test@example.com", UserRole.Graduate);

        // Assert
        user.Email.Should().Be("test@example.com");
        user.Role.Equals(UserRole.Graduate).Should().BeTrue();
        user.IsActive.Should().BeTrue();
        user.CreatedAt.Should().Be(DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Create_WithInvalidEmail_ThrowsArgumentException(string email)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => User.Create(email!, UserRole.Graduate));
    }

    [Fact]
    public async Task ChangeRole_UpdatesRoleAndTimestamp()
    {
        // Arrange
        var user = User.Create("test@example.com", UserRole.Graduate);
        var initialTime = user.UpdatedAt;

        // Act
        await Task.Delay(5); // Гарантируем разницу во времени
        user.ChangeRole(UserRole.Mentor);

        // Assert
        Assert.Equal(UserRole.Mentor, user.Role);
        Assert.True(user.UpdatedAt > initialTime);
    }

    [Fact]
    public void ChangeRole_ToSameRole_DoesNotUpdateTimestamp()
    {
        // Arrange
        var user = User.Create("test@example.com", UserRole.Graduate);

        // Act
        user.ChangeRole(UserRole.Graduate);

        // Assert
        Assert.Null(user.UpdatedAt); // Timestamp не должен измениться
    }
}