using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Chat;

public class SendMessageRequest
{
    [Required]
    public required string RecipientId { get; set; }

    [Required]
    public required string Content { get; set; }
}

