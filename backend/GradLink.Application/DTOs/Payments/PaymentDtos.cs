namespace GradLink.Application.DTOs.Payments;

/// <summary>
/// Request to create a payment intent
/// </summary>
public class CreatePaymentIntentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "usd";
    public string? Description { get; set; }
    public int? SponsorshipId { get; set; }
    public int? ProjectId { get; set; }
}

/// <summary>
/// Response for payment intent creation
/// </summary>
public class PaymentIntentResponse
{
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// DTO for a payment transaction
/// </summary>
public class PaymentDto
{
    public int Id { get; set; }
    public string PaymentIntentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SponsorshipId { get; set; }
    public int? ProjectId { get; set; }
    public string? ProjectTitle { get; set; }
    public string PayerId { get; set; } = string.Empty;
    public string? PayerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Request to confirm a payment
/// </summary>
public class ConfirmPaymentRequest
{
    public string PaymentIntentId { get; set; } = string.Empty;
}

/// <summary>
/// Webhook payload from Stripe
/// </summary>
public class StripeWebhookPayload
{
    public string Type { get; set; } = string.Empty;
    public object Data { get; set; } = new();
}















