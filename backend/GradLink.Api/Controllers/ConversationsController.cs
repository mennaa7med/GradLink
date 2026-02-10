using GradLink.Application.DTOs.Chat;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ConversationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConversationDto>>> GetConversations()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var conversations = await _context.Conversations
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .Select(c => new ConversationDto
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                LastMessageAt = c.LastMessageAt,
                User1Id = c.User1Id,
                User1Name = c.User1!.FullName,
                User2Id = c.User2Id,
                User2Name = c.User2!.FullName
            })
            .ToListAsync();

        return Ok(conversations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConversationDto>> GetConversation(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var conversation = await _context.Conversations
            .Where(c => c.Id == id && (c.User1Id == userId || c.User2Id == userId))
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync();

        if (conversation == null)
        {
            return NotFound();
        }

        var conversationDto = new ConversationDto
        {
            Id = conversation.Id,
            CreatedAt = conversation.CreatedAt,
            LastMessageAt = conversation.LastMessageAt,
            User1Id = conversation.User1Id,
            User1Name = conversation.User1!.FullName,
            User2Id = conversation.User2Id,
            User2Name = conversation.User2!.FullName,
            Messages = conversation.Messages
                .OrderBy(m => m.SentAt)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead,
                    SenderId = m.SenderId,
                    SenderName = m.Sender!.FullName
                })
                .ToList()
        };

        // Mark messages as read
        var unreadMessages = conversation.Messages
            .Where(m => m.SenderId != userId && !m.IsRead)
            .ToList();

        foreach (var message in unreadMessages)
        {
            message.IsRead = true;
        }

        if (unreadMessages.Any())
        {
            await _context.SaveChangesAsync();
        }

        return Ok(conversationDto);
    }

    /// <summary>
    /// Get unread messages count for current user
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<ActionResult<object>> GetUnreadMessagesCount()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var unreadCount = await _context.ChatMessages
            .Where(m => m.Conversation != null && 
                        (m.Conversation.User1Id == userId || m.Conversation.User2Id == userId) &&
                        m.SenderId != userId && 
                        !m.IsRead)
            .CountAsync();

        return Ok(new { unreadCount });
    }

    /// <summary>
    /// Get recent unread messages for current user
    /// </summary>
    [HttpGet("unread-messages")]
    public async Task<ActionResult<object>> GetUnreadMessages([FromQuery] int limit = 5)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var unreadMessages = await _context.ChatMessages
            .Where(m => m.Conversation != null && 
                        (m.Conversation.User1Id == userId || m.Conversation.User2Id == userId) &&
                        m.SenderId != userId && 
                        !m.IsRead)
            .OrderByDescending(m => m.SentAt)
            .Take(limit)
            .Include(m => m.Sender)
            .Include(m => m.Conversation)
            .Select(m => new {
                m.Id,
                m.Content,
                m.SentAt,
                m.ConversationId,
                SenderName = m.Sender != null ? m.Sender.FullName : "Unknown",
                SenderAvatar = m.Sender != null ? m.Sender.ProfilePicture : null
            })
            .ToListAsync();

        return Ok(unreadMessages);
    }

    [HttpPost]
    public async Task<ActionResult<ConversationDto>> CreateOrGetConversation([FromBody] CreateConversationRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        // Check if conversation already exists
        var existingConversation = await _context.Conversations
            .Where(c => (c.User1Id == userId && c.User2Id == request.RecipientId) ||
                        (c.User1Id == request.RecipientId && c.User2Id == userId))
            .Include(c => c.User1)
            .Include(c => c.User2)
            .FirstOrDefaultAsync();

        if (existingConversation != null)
        {
            return Ok(new ConversationDto
            {
                Id = existingConversation.Id,
                CreatedAt = existingConversation.CreatedAt,
                LastMessageAt = existingConversation.LastMessageAt,
                User1Id = existingConversation.User1Id,
                User1Name = existingConversation.User1!.FullName,
                User2Id = existingConversation.User2Id,
                User2Name = existingConversation.User2!.FullName
            });
        }

        // Create new conversation
        var conversation = new Conversation
        {
            User1Id = userId,
            User2Id = request.RecipientId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        // Load users
        await _context.Entry(conversation).Reference(c => c.User1).LoadAsync();
        await _context.Entry(conversation).Reference(c => c.User2).LoadAsync();

        return CreatedAtAction(nameof(GetConversation), new { id = conversation.Id }, new ConversationDto
        {
            Id = conversation.Id,
            CreatedAt = conversation.CreatedAt,
            LastMessageAt = conversation.LastMessageAt,
            User1Id = conversation.User1Id,
            User1Name = conversation.User1!.FullName,
            User2Id = conversation.User2Id,
            User2Name = conversation.User2!.FullName
        });
    }
}

public class CreateConversationRequest
{
    public required string RecipientId { get; set; }
}

