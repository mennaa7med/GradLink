using GradLink.Application.DTOs.Notifications;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(AppDbContext context, ILogger<NotificationsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all notifications for current user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications(
        [FromQuery] bool? unreadOnly,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var query = _context.Notifications.Where(n => n.UserId == userId);

        if (unreadOnly == true)
        {
            query = query.Where(n => !n.IsRead);
        }

        var total = await query.CountAsync();
        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Type = n.Type,
                Title = n.Title,
                Message = n.Message,
                Link = n.Link,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                ReadAt = n.ReadAt,
                RelatedEntityType = n.RelatedEntityType,
                RelatedEntityId = n.RelatedEntityId
            })
            .ToListAsync();

        return Ok(new { total, page, pageSize, notifications });
    }

    /// <summary>
    /// Get notification count
    /// </summary>
    [HttpGet("count")]
    public async Task<ActionResult<NotificationCountDto>> GetNotificationCount()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var total = await _context.Notifications.CountAsync(n => n.UserId == userId);
        var unread = await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

        return Ok(new NotificationCountDto { Total = total, Unread = unread });
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (notification == null) return NotFound();

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Mark all notifications as read
    /// </summary>
    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete notification
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (notification == null) return NotFound();

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete all read notifications
    /// </summary>
    [HttpDelete("clear-read")]
    public async Task<IActionResult> ClearReadNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var readNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && n.IsRead)
            .ToListAsync();

        _context.Notifications.RemoveRange(readNotifications);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Get push notification configuration
    /// </summary>
    [HttpGet("push/config")]
    [AllowAnonymous]
    public IActionResult GetPushConfig([FromServices] IConfiguration config)
    {
        // VAPID key should be generated and stored in configuration
        var vapidPublicKey = config["Push:VapidPublicKey"] ?? "";
        
        return Ok(new { vapidPublicKey });
    }

    /// <summary>
    /// Subscribe to push notifications
    /// </summary>
    [HttpPost("push/subscribe")]
    public async Task<IActionResult> SubscribeToPush([FromBody] PushSubscriptionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        // Store subscription in database
        var existing = await _context.Set<PushSubscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == request.Endpoint);

        if (existing == null)
        {
            _context.Set<PushSubscription>().Add(new PushSubscription
            {
                UserId = userId,
                Endpoint = request.Endpoint,
                P256dh = request.Keys?.P256dh ?? "",
                Auth = request.Keys?.Auth ?? "",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        _logger.LogInformation("Push subscription added for user {UserId}", userId);
        return Ok(new { message = "Subscribed to push notifications" });
    }

    /// <summary>
    /// Unsubscribe from push notifications
    /// </summary>
    [HttpPost("push/unsubscribe")]
    public async Task<IActionResult> UnsubscribeFromPush([FromBody] PushUnsubscribeRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var subscription = await _context.Set<PushSubscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == request.Endpoint);

        if (subscription != null)
        {
            _context.Set<PushSubscription>().Remove(subscription);
            await _context.SaveChangesAsync();
        }

        _logger.LogInformation("Push subscription removed for user {UserId}", userId);
        return Ok(new { message = "Unsubscribed from push notifications" });
    }
}

public class PushSubscriptionRequest
{
    public string Endpoint { get; set; } = string.Empty;
    public PushKeys? Keys { get; set; }
}

public class PushKeys
{
    public string P256dh { get; set; } = string.Empty;
    public string Auth { get; set; } = string.Empty;
}

public class PushUnsubscribeRequest
{
    public string Endpoint { get; set; } = string.Empty;
}



