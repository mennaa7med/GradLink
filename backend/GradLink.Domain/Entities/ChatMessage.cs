namespace GradLink.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    // Foreign keys
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }

    public required string SenderId { get; set; }
    public ApplicationUser? Sender { get; set; }
}

