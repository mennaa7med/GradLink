using GradLink.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace GradLink.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly bool _enabled;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _smtpHost = configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
        _smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "587");
        _smtpUsername = configuration["Email:Username"] ?? "";
        _smtpPassword = configuration["Email:Password"] ?? "";
        _fromEmail = configuration["Email:FromEmail"] ?? "noreply@gradlink.com";
        _fromName = configuration["Email:FromName"] ?? "GradLink";
        _enabled = !string.IsNullOrEmpty(_smtpUsername) && !string.IsNullOrEmpty(_smtpPassword);
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        if (!_enabled)
        {
            _logger.LogWarning("Email service is not configured. Would send to {To}: {Subject}", to, subject);
            return;
        }

        try
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent to {To}: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}: {Subject}", to, subject);
            throw;
        }
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        var subject = "🔐 GradLink - Reset Your Password";
        var htmlBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        body {{ font-family: 'Segoe UI', 'Roboto', 'Helvetica Neue', Arial, sans-serif; line-height: 1.6; color: #1a1a2e; background-color: #f0f2f5; }}
        .wrapper {{ max-width: 600px; margin: 0 auto; padding: 40px 20px; }}
        .email-container {{ background: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 40px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 50%, #a855f7 100%); padding: 40px 30px; text-align: center; }}
        .logo-container {{ display: flex; align-items: center; justify-content: center; gap: 12px; margin-bottom: 10px; }}
        .logo {{ width: 50px; height: 50px; background: white; border-radius: 12px; display: flex; align-items: center; justify-content: center; font-size: 28px; }}
        .brand-name {{ color: white; font-size: 32px; font-weight: 700; letter-spacing: -1px; }}
        .header-subtitle {{ color: rgba(255,255,255,0.9); font-size: 16px; margin-top: 8px; }}
        .content {{ padding: 40px 35px; }}
        .greeting {{ font-size: 24px; font-weight: 600; color: #1a1a2e; margin-bottom: 20px; }}
        .message {{ color: #4a5568; font-size: 16px; margin-bottom: 15px; line-height: 1.7; }}
        .button-container {{ text-align: center; margin: 35px 0; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%); color: white !important; padding: 16px 40px; text-decoration: none; border-radius: 50px; font-size: 16px; font-weight: 600; letter-spacing: 0.5px; box-shadow: 0 4px 15px rgba(99, 102, 241, 0.4); transition: all 0.3s ease; }}
        .button:hover {{ box-shadow: 0 6px 20px rgba(99, 102, 241, 0.5); transform: translateY(-2px); }}
        .divider {{ height: 1px; background: linear-gradient(to right, transparent, #e2e8f0, transparent); margin: 30px 0; }}
        .info-box {{ background: #f8fafc; border-left: 4px solid #6366f1; padding: 15px 20px; border-radius: 0 8px 8px 0; margin: 20px 0; }}
        .info-box p {{ color: #64748b; font-size: 14px; margin: 0; }}
        .security-note {{ background: #fef3c7; border-radius: 8px; padding: 15px; margin-top: 25px; }}
        .security-note p {{ color: #92400e; font-size: 14px; margin: 0; display: flex; align-items: center; gap: 8px; }}
        .footer {{ background: #f8fafc; padding: 30px; text-align: center; border-top: 1px solid #e2e8f0; }}
        .footer-logo {{ display: flex; align-items: center; justify-content: center; gap: 8px; margin-bottom: 15px; }}
        .footer-logo-icon {{ font-size: 20px; }}
        .footer-logo-text {{ font-size: 18px; font-weight: 600; color: #6366f1; }}
        .footer-text {{ color: #94a3b8; font-size: 13px; margin-bottom: 10px; }}
        .social-links {{ margin-top: 15px; }}
        .social-links a {{ display: inline-block; margin: 0 8px; color: #6366f1; text-decoration: none; font-size: 14px; }}
    </style>
</head>
<body>
    <div class='wrapper'>
        <div class='email-container'>
            <div class='header'>
                <div class='logo-container'>
                    <div class='logo'>🎓</div>
                    <span class='brand-name'>GradLink</span>
                </div>
                <p class='header-subtitle'>Connecting Graduates with Opportunities</p>
            </div>
            <div class='content'>
                <h2 class='greeting'>Password Reset Request 🔐</h2>
                <p class='message'>Hi there! 👋</p>
                <p class='message'>We received a request to reset the password for your <strong>GradLink</strong> account. No worries, it happens to the best of us!</p>
                
                <div class='button-container'>
                    <a href='{resetLink}' class='button'>🔑 Reset My Password</a>
                </div>
                
                <div class='info-box'>
                    <p>⏰ This link will expire in <strong>1 hour</strong> for security reasons.</p>
                </div>
                
                <div class='divider'></div>
                
                <p class='message'>If clicking the button doesn't work, copy and paste this link into your browser:</p>
                <p style='word-break: break-all; color: #6366f1; font-size: 14px; background: #f1f5f9; padding: 12px; border-radius: 8px;'>{resetLink}</p>
                
                <div class='security-note'>
                    <p>🛡️ <strong>Security Tip:</strong> If you didn't request this password reset, please ignore this email. Your password will remain unchanged.</p>
                </div>
            </div>
            <div class='footer'>
                <div class='footer-logo'>
                    <span class='footer-logo-icon'>🎓</span>
                    <span class='footer-logo-text'>GradLink</span>
                </div>
                <p class='footer-text'>Your gateway to career opportunities</p>
                <p class='footer-text'>&copy; 2024 GradLink. All rights reserved.</p>
                <div class='social-links'>
                    <a href='#'>About Us</a> •
                    <a href='#'>Contact</a> •
                    <a href='#'>Privacy Policy</a>
                </div>
            </div>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(to, subject, htmlBody);
    }

    public async Task SendWelcomeEmailAsync(string to, string userName)
    {
        var subject = "🎉 Welcome to GradLink - Your Career Journey Starts Here!";
        var htmlBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        body {{ font-family: 'Segoe UI', 'Roboto', 'Helvetica Neue', Arial, sans-serif; line-height: 1.6; color: #1a1a2e; background-color: #f0f2f5; }}
        .wrapper {{ max-width: 600px; margin: 0 auto; padding: 40px 20px; }}
        .email-container {{ background: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 40px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 50%, #047857 100%); padding: 40px 30px; text-align: center; }}
        .logo-container {{ display: flex; align-items: center; justify-content: center; gap: 12px; margin-bottom: 10px; }}
        .logo {{ width: 50px; height: 50px; background: white; border-radius: 12px; display: flex; align-items: center; justify-content: center; font-size: 28px; }}
        .brand-name {{ color: white; font-size: 32px; font-weight: 700; letter-spacing: -1px; }}
        .header-subtitle {{ color: rgba(255,255,255,0.9); font-size: 16px; margin-top: 8px; }}
        .welcome-badge {{ display: inline-block; background: rgba(255,255,255,0.2); color: white; padding: 8px 20px; border-radius: 50px; font-size: 14px; margin-top: 15px; }}
        .content {{ padding: 40px 35px; }}
        .greeting {{ font-size: 26px; font-weight: 600; color: #1a1a2e; margin-bottom: 20px; }}
        .message {{ color: #4a5568; font-size: 16px; margin-bottom: 15px; line-height: 1.7; }}
        .features {{ margin: 30px 0; }}
        .feature {{ display: flex; align-items: center; gap: 15px; padding: 15px; background: #f8fafc; border-radius: 12px; margin-bottom: 12px; }}
        .feature-icon {{ width: 45px; height: 45px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-size: 22px; }}
        .feature-icon.jobs {{ background: linear-gradient(135deg, #6366f1, #8b5cf6); }}
        .feature-icon.mentors {{ background: linear-gradient(135deg, #f59e0b, #d97706); }}
        .feature-icon.projects {{ background: linear-gradient(135deg, #10b981, #059669); }}
        .feature-icon.resources {{ background: linear-gradient(135deg, #ec4899, #db2777); }}
        .feature-text {{ flex: 1; }}
        .feature-title {{ font-weight: 600; color: #1a1a2e; font-size: 15px; }}
        .feature-desc {{ color: #64748b; font-size: 13px; }}
        .divider {{ height: 1px; background: linear-gradient(to right, transparent, #e2e8f0, transparent); margin: 30px 0; }}
        .footer {{ background: #f8fafc; padding: 30px; text-align: center; border-top: 1px solid #e2e8f0; }}
        .footer-logo {{ display: flex; align-items: center; justify-content: center; gap: 8px; margin-bottom: 15px; }}
        .footer-logo-icon {{ font-size: 20px; }}
        .footer-logo-text {{ font-size: 18px; font-weight: 600; color: #10b981; }}
        .footer-text {{ color: #94a3b8; font-size: 13px; margin-bottom: 10px; }}
    </style>
</head>
<body>
    <div class='wrapper'>
        <div class='email-container'>
            <div class='header'>
                <div class='logo-container'>
                    <div class='logo'>🎓</div>
                    <span class='brand-name'>GradLink</span>
                </div>
                <p class='header-subtitle'>Connecting Graduates with Opportunities</p>
                <span class='welcome-badge'>🎉 Welcome Aboard!</span>
            </div>
            <div class='content'>
                <h2 class='greeting'>Hello {userName}! 👋</h2>
                <p class='message'>Welcome to <strong>GradLink</strong>! We're thrilled to have you join our community.</p>
                <p class='message'>Here's what you can do:</p>
                
                <div class='features'>
                    <div class='feature'>
                        <div class='feature-icon jobs'>💼</div>
                        <div class='feature-text'>
                            <div class='feature-title'>Find Jobs & Internships</div>
                            <div class='feature-desc'>Discover opportunities from top companies</div>
                        </div>
                    </div>
                    <div class='feature'>
                        <div class='feature-icon mentors'>🤝</div>
                        <div class='feature-text'>
                            <div class='feature-title'>Connect with Mentors</div>
                            <div class='feature-desc'>Get guidance from industry professionals</div>
                        </div>
                    </div>
                    <div class='feature'>
                        <div class='feature-icon projects'>📂</div>
                        <div class='feature-text'>
                            <div class='feature-title'>Manage Projects</div>
                            <div class='feature-desc'>Organize your graduation projects easily</div>
                        </div>
                    </div>
                    <div class='feature'>
                        <div class='feature-icon resources'>📚</div>
                        <div class='feature-text'>
                            <div class='feature-title'>Access Resources</div>
                            <div class='feature-desc'>Learn and grow with curated materials</div>
                        </div>
                    </div>
                </div>
                
                <div class='divider'></div>
                
                <p class='message' style='text-align: center;'>Ready to take the first step towards your dream career? Let's go! 💪</p>
            </div>
            <div class='footer'>
                <div class='footer-logo'>
                    <span class='footer-logo-icon'>🎓</span>
                    <span class='footer-logo-text'>GradLink</span>
                </div>
                <p class='footer-text'>Your gateway to career opportunities</p>
                <p class='footer-text'>&copy; 2024 GradLink. All rights reserved.</p>
            </div>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(to, subject, htmlBody);
    }

    public async Task SendApplicationNotificationAsync(string to, string applicantName, string opportunityTitle)
    {
        var subject = $"New Application Received - {opportunityTitle}";
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .highlight {{ background: #e0f2fe; padding: 15px; border-radius: 5px; margin: 15px 0; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📬 New Application</h1>
        </div>
        <div class='content'>
            <h2>You have a new applicant!</h2>
            <div class='highlight'>
                <strong>{applicantName}</strong> has applied for <strong>{opportunityTitle}</strong>
            </div>
            <p>Log in to your dashboard to review this application and take action.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 GradLink. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(to, subject, htmlBody);
    }

    public async Task SendApplicationStatusUpdateAsync(string to, string opportunityTitle, string status)
    {
        var statusEmoji = status.ToLower() switch
        {
            "accepted" => "🎉",
            "shortlisted" => "⭐",
            "reviewed" => "👀",
            "rejected" => "😔",
            _ => "📋"
        };

        var subject = $"Application Update - {opportunityTitle}";
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .status {{ background: #e0f2fe; padding: 20px; border-radius: 5px; margin: 15px 0; text-align: center; font-size: 24px; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>{statusEmoji} Application Update</h1>
        </div>
        <div class='content'>
            <h2>Your application status has been updated</h2>
            <p>Your application for <strong>{opportunityTitle}</strong> has been updated:</p>
            <div class='status'>
                Status: <strong>{status}</strong>
            </div>
            <p>Log in to your dashboard for more details.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 GradLink. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(to, subject, htmlBody);
    }
}

