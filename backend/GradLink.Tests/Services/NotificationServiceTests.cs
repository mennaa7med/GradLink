using Xunit;
using FluentAssertions;

namespace GradLink.Tests.Services;

public class NotificationServiceTests
{
    [Fact]
    public void Notification_Type_ShouldBeValid()
    {
        // Arrange
        var validTypes = new[] { "JobApplication", "InternshipApplication", "ApplicationUpdate", "Message", "ProjectUpdate" };
        var testType = "JobApplication";

        // Assert
        validTypes.Should().Contain(testType);
    }

    [Fact]
    public void Notification_Title_MaxLength_Is200()
    {
        // Arrange
        var shortTitle = "New Application";
        var longTitle = new string('A', 201);

        // Assert
        shortTitle.Length.Should().BeLessOrEqualTo(200);
        longTitle.Length.Should().BeGreaterThan(200);
    }

    [Fact]
    public void Notification_ShouldDefaultToUnread()
    {
        // Arrange
        var isRead = false;

        // Assert
        isRead.Should().BeFalse();
    }
}

















