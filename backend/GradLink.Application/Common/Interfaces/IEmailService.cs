namespace GradLink.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
    Task SendPasswordResetEmailAsync(string to, string resetLink);
    Task SendWelcomeEmailAsync(string to, string userName);
    Task SendApplicationNotificationAsync(string to, string applicantName, string opportunityTitle);
    Task SendApplicationStatusUpdateAsync(string to, string opportunityTitle, string status);
}

















