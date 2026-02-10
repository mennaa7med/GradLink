namespace GradLink.Application.DTOs.Chat;

public class ConversationDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public required string User1Id { get; set; }
    public string? User1Name { get; set; }
    public required string User2Id { get; set; }
    public string? User2Name { get; set; }
    public List<ChatMessageDto> Messages { get; set; } = new();
}

public class ChatMessageDto
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
    public required string SenderId { get; set; }
    public string? SenderName { get; set; }
}

