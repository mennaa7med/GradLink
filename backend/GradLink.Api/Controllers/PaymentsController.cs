using GradLink.Application.DTOs.Payments;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for payment processing (Stripe integration)
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<PaymentsController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Create a payment intent for sponsorship
    /// </summary>
    [HttpPost("create-intent")]
    public async Task<ActionResult<PaymentIntentResponse>> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var stripeSecretKey = _configuration["Stripe:SecretKey"];
        
        // For demo purposes without Stripe SDK
        // In production, you would use Stripe.NET SDK
        if (string.IsNullOrEmpty(stripeSecretKey))
        {
            _logger.LogWarning("Stripe is not configured. Creating mock payment intent.");
            
            // Create a mock payment record
            var mockPayment = new Payment
            {
                PaymentIntentId = $"pi_mock_{Guid.NewGuid():N}",
                Amount = request.Amount,
                Currency = request.Currency.ToLower(),
                Status = "requires_confirmation",
                Description = request.Description,
                SponsorshipId = request.SponsorshipId,
                ProjectId = request.ProjectId,
                PayerId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(mockPayment);
            await _context.SaveChangesAsync();

            return Ok(new PaymentIntentResponse
            {
                ClientSecret = $"mock_secret_{mockPayment.Id}",
                PaymentIntentId = mockPayment.PaymentIntentId,
                Amount = request.Amount,
                Currency = request.Currency.ToLower(),
                Status = "requires_confirmation"
            });
        }

        // TODO: Implement actual Stripe payment intent creation
        // This would use Stripe.NET SDK:
        // var service = new PaymentIntentService();
        // var options = new PaymentIntentCreateOptions { ... };
        // var paymentIntent = await service.CreateAsync(options);

        return BadRequest(new { error = "Stripe integration pending. Please configure Stripe API keys." });
    }

    /// <summary>
    /// Confirm a payment (for demo/mock purposes)
    /// </summary>
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentIntentId == request.PaymentIntentId && p.PayerId == userId);

        if (payment == null)
        {
            return NotFound(new { error = "Payment not found" });
        }

        if (payment.Status == "succeeded")
        {
            return BadRequest(new { error = "Payment already completed" });
        }

        // Mark payment as succeeded
        payment.Status = "succeeded";
        payment.CompletedAt = DateTime.UtcNow;

        // Update sponsorship if applicable
        if (payment.SponsorshipId.HasValue)
        {
            var sponsorship = await _context.SponsoredProjects.FindAsync(payment.SponsorshipId);
            if (sponsorship != null)
            {
                sponsorship.FundingDelivered = (sponsorship.FundingDelivered ?? 0) + payment.Amount;
                sponsorship.Status = "Active";
            }
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Payment {PaymentIntentId} confirmed for user {UserId}", 
            request.PaymentIntentId, userId);

        return Ok(new { message = "Payment confirmed successfully", status = payment.Status });
    }

    /// <summary>
    /// Get payment history for current user
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<List<PaymentDto>>> GetPaymentHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var payments = await _context.Payments
            .Where(p => p.PayerId == userId)
            .Include(p => p.Project)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                PaymentIntentId = p.PaymentIntentId,
                Amount = p.Amount,
                Currency = p.Currency,
                Status = p.Status,
                Description = p.Description,
                SponsorshipId = p.SponsorshipId,
                ProjectId = p.ProjectId,
                ProjectTitle = p.Project != null ? p.Project.Title : null,
                PayerId = p.PayerId,
                CreatedAt = p.CreatedAt,
                CompletedAt = p.CompletedAt
            })
            .ToListAsync();

        var total = await _context.Payments.CountAsync(p => p.PayerId == userId);

        return Ok(new { payments, total, page, pageSize });
    }

    /// <summary>
    /// Get payment configuration (publishable key for frontend)
    /// </summary>
    [HttpGet("config")]
    [AllowAnonymous]
    public IActionResult GetPaymentConfig()
    {
        var publishableKey = _configuration["Stripe:PublishableKey"] ?? "";
        var isConfigured = !string.IsNullOrEmpty(_configuration["Stripe:SecretKey"]);

        return Ok(new 
        { 
            publishableKey,
            isConfigured,
            currency = "usd",
            minimumAmount = 1m
        });
    }

    /// <summary>
    /// Stripe webhook handler
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> HandleWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var webhookSecret = _configuration["Stripe:WebhookSecret"];

        _logger.LogInformation("Received Stripe webhook");

        // TODO: Validate webhook signature using Stripe SDK
        // In production, verify the event came from Stripe

        try
        {
            // Parse and handle the event
            // This is a placeholder - use Stripe.NET SDK for proper parsing
            
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Stripe webhook");
            return BadRequest(new { error = "Webhook processing failed" });
        }
    }
}

