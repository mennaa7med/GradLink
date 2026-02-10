using Microsoft.AspNetCore.Mvc;
using GradLink.Application.Common.Interfaces;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for handling contact form submissions
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Contact")]
public class ContactController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactController> _logger;
    private readonly IConfiguration _configuration;

    public ContactController(
        IEmailService emailService,
        ILogger<ContactController> logger,
        IConfiguration configuration)
    {
        _emailService = emailService;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Submit a contact form
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SubmitContactForm([FromBody] ContactFormRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Get support email from configuration
            var supportEmail = _configuration["Email:SupportEmail"] ?? "mennaahmedfawzy10@gmail.com";
            
            // Build email content
            var subject = $"[GradLink Contact] {request.Subject} - From {request.Name}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: linear-gradient(135deg, #0c2737 0%, #1a3a4a 100%); padding: 30px; text-align: center;'>
                        <h1 style='color: #f0ad4e; margin: 0;'>New Contact Form Submission</h1>
                    </div>
                    <div style='background: #f8f9fa; padding: 30px;'>
                        <h2 style='color: #0c2737; margin-top: 0;'>Message Details</h2>
                        <p><strong>Name:</strong> {request.Name}</p>
                        <p><strong>Email:</strong> {request.Email}</p>
                        <p><strong>Subject:</strong> {request.Subject}</p>
                        <p><strong>Message:</strong></p>
                        <div style='background: white; padding: 20px; border-radius: 8px; border-left: 4px solid #f0ad4e;'>
                            {request.Message.Replace("\n", "<br>")}
                        </div>
                        <p style='margin-top: 20px; font-size: 12px; color: #6b7280;'>
                            Submitted at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC
                        </p>
                    </div>
                </div>
            ";

            // Send email to support
            await _emailService.SendEmailAsync(supportEmail, subject, body);
            
            // Send confirmation to user
            var confirmationSubject = "Thank you for contacting GradLink";
            var confirmationBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: linear-gradient(135deg, #0c2737 0%, #1a3a4a 100%); padding: 30px; text-align: center;'>
                        <h1 style='color: #f0ad4e; margin: 0;'>🎓 GradLink</h1>
                    </div>
                    <div style='padding: 30px;'>
                        <h2 style='color: #0c2737;'>Hi {request.Name},</h2>
                        <p>Thank you for reaching out to us! We've received your message and will get back to you within 24-48 hours.</p>
                        <div style='background: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                            <strong>Your message summary:</strong>
                            <p style='color: #6b7280;'>{request.Message.Substring(0, Math.Min(200, request.Message.Length))}...</p>
                        </div>
                        <p>In the meantime, feel free to explore our platform:</p>
                        <ul>
                            <li>Browse <a href='#' style='color: #f0ad4e;'>Career Opportunities</a></li>
                            <li>Explore <a href='#' style='color: #f0ad4e;'>Projects Bank</a></li>
                            <li>Connect with <a href='#' style='color: #f0ad4e;'>Mentors</a></li>
                        </ul>
                        <p>Best regards,<br>The GradLink Team</p>
                    </div>
                </div>
            ";

            await _emailService.SendEmailAsync(request.Email, confirmationSubject, confirmationBody);

            _logger.LogInformation("Contact form submitted successfully from {Email}", request.Email);
            
            return Ok(new { message = "Your message has been sent successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process contact form from {Email}", request.Email);
            return StatusCode(500, new { message = "Failed to send message. Please try again later." });
        }
    }
}

public class ContactFormRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
}















