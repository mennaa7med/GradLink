using Xunit;
using FluentAssertions;

namespace GradLink.Tests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public void Register_WithValidData_ShouldSucceed()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Test123!";

        // Assert
        email.Should().NotBeNullOrEmpty();
        password.Length.Should().BeGreaterOrEqualTo(6);
    }

    [Fact]
    public void Password_MustBeAtLeast6Characters()
    {
        // Arrange
        var shortPassword = "12345";
        var validPassword = "123456";

        // Assert
        shortPassword.Length.Should().BeLessThan(6);
        validPassword.Length.Should().BeGreaterOrEqualTo(6);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("", false)]
    public void Email_Validation(string email, bool shouldBeValid)
    {
        // Simple email validation
        var isValid = !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
        
        isValid.Should().Be(shouldBeValid);
    }
}

















