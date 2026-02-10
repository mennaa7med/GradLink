namespace GradLink.Domain.Entities;

public class Conversation
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastMessageAt { get; set; }

    // Foreign keys
    public required string User1Id { get; set; }
    public ApplicationUser? User1 { get; set; }

    public required string User2Id { get; set; }
    public ApplicationUser? User2 { get; set; }

    // Navigation properties
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}

