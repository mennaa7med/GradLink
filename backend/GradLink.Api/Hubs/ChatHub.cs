using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(AppDbContext context, ILogger<ChatHub> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            _logger.LogInformation("User {UserId} connected to chat hub", userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            _logger.LogInformation("User {UserId} disconnected from chat hub", userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string recipientId, string message)
    {
        var senderId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (senderId == null)
        {
            throw new HubException("Unauthorized");
        }

        // Find or create conversation
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                (c.User1Id == senderId && c.User2Id == recipientId) ||
                (c.User1Id == recipientId && c.User2Id == senderId));

        if (conversation == null)
        {
            conversation = new Conversation
            {
                User1Id = senderId,
                User2Id = recipientId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        // Create message
        var chatMessage = new ChatMessage
        {
            ConversationId = conversation.Id,
            SenderId = senderId,
            Content = message,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.ChatMessages.Add(chatMessage);

        conversation.LastMessageAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Load sender info
        await _context.Entry(chatMessage).Reference(m => m.Sender).LoadAsync();

        // Send to recipient
        await Clients.Group(recipientId).SendAsync("ReceiveMessage", new
        {
            chatMessage.Id,
            chatMessage.Content,
            chatMessage.SentAt,
            chatMessage.IsRead,
            chatMessage.SenderId,
            SenderName = chatMessage.Sender!.FullName,
            chatMessage.ConversationId
        });

        // Echo back to sender
        await Clients.Caller.SendAsync("MessageSent", new
        {
            chatMessage.Id,
            chatMessage.Content,
            chatMessage.SentAt,
            chatMessage.IsRead,
            chatMessage.SenderId,
            SenderName = chatMessage.Sender!.FullName,
            chatMessage.ConversationId
        });

        _logger.LogInformation("Message sent from {SenderId} to {RecipientId}", senderId, recipientId);
    }

    public async Task MarkAsRead(int conversationId)
    {
        var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new HubException("Unauthorized");
        }

        var messages = await _context.ChatMessages
            .Where(m => m.ConversationId == conversationId && m.SenderId != userId && !m.IsRead)
            .ToListAsync();

        foreach (var message in messages)
        {
            message.IsRead = true;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Marked {Count} messages as read in conversation {ConversationId}", messages.Count, conversationId);
    }
}

