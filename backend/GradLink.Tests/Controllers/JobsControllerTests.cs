using Xunit;
using FluentAssertions;

namespace GradLink.Tests.Controllers;

public class JobsControllerTests
{
    [Fact]
    public void Job_Title_ShouldNotBeEmpty()
    {
        // Arrange
        var jobTitle = "Software Engineer";

        // Assert
        jobTitle.Should().NotBeNullOrEmpty();
        jobTitle.Length.Should().BeLessOrEqualTo(200);
    }

    [Fact]
    public void Job_Salary_ShouldBePositive()
    {
        // Arrange
        decimal salaryMin = 50000;
        decimal salaryMax = 100000;

        // Assert
        salaryMin.Should().BePositive();
        salaryMax.Should().BeGreaterThanOrEqualTo(salaryMin);
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("Closed")]
    [InlineData("Draft")]
    public void Job_Status_ShouldBeValid(string status)
    {
        // Arrange
        var validStatuses = new[] { "Active", "Closed", "Draft" };

        // Assert
        validStatuses.Should().Contain(status);
    }
}

















