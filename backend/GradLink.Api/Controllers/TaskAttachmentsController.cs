using GradLink.Application.Common.Interfaces;
using GradLink.Application.DTOs.Tasks;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for managing task attachments/files
/// </summary>
[Authorize]
[ApiController]
[Route("api/tasks/{taskId}/attachments")]
public class TaskAttachmentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<TaskAttachmentsController> _logger;

    private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".jpg", ".jpeg", ".png", ".gif", ".zip", ".rar", ".7z" };
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public TaskAttachmentsController(
        AppDbContext context,
        IFileStorage fileStorage,
        ILogger<TaskAttachmentsController> logger)
    {
        _context = context;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    /// <summary>
    /// Get all attachments for a task
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<TaskAttachmentDto>>> GetAttachments(int taskId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

        if (task == null)
        {
            return NotFound(new { error = "Task not found" });
        }

        var attachments = await _context.TaskAttachments
            .Where(a => a.TaskId == taskId)
            .Include(a => a.UploadedBy)
            .Select(a => new TaskAttachmentDto
            {
                Id = a.Id,
                TaskId = a.TaskId,
                FileName = a.FileName,
                OriginalFileName = a.OriginalFileName,
                ContentType = a.ContentType,
                FileSize = a.FileSize,
                Description = a.Description,
                UploadedById = a.UploadedById,
                UploadedByName = a.UploadedBy != null ? a.UploadedBy.FullName : null,
                UploadedAt = a.UploadedAt,
                DownloadUrl = $"/api/tasks/{taskId}/attachments/{a.Id}/download"
            })
            .ToListAsync();

        return Ok(attachments);
    }

    /// <summary>
    /// Upload attachment to a task
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskAttachmentDto>> UploadAttachment(int taskId, IFormFile file, [FromForm] string? description)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

        if (task == null)
        {
            return NotFound(new { error = "Task not found" });
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "No file provided" });
        }

        if (file.Length > MaxFileSize)
        {
            return BadRequest(new { error = $"File size exceeds limit of {MaxFileSize / 1024 / 1024}MB" });
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            return BadRequest(new { error = $"File type '{extension}' is not allowed" });
        }

        try
        {
            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var storagePath = $"task-attachments/{taskId}/{fileName}";

            // Upload to storage
            using var stream = file.OpenReadStream();
            await _fileStorage.SaveFileAsync(stream, fileName, $"task-attachments/{taskId}");

            // Create attachment record
            var attachment = new TaskAttachment
            {
                TaskId = taskId,
                FileName = fileName,
                OriginalFileName = file.FileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                StoragePath = storagePath,
                Description = description,
                UploadedById = userId!,
                UploadedAt = DateTime.UtcNow
            };

            _context.TaskAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("File uploaded for task {TaskId}: {FileName}", taskId, file.FileName);

            return Ok(new TaskAttachmentDto
            {
                Id = attachment.Id,
                TaskId = attachment.TaskId,
                FileName = attachment.FileName,
                OriginalFileName = attachment.OriginalFileName,
                ContentType = attachment.ContentType,
                FileSize = attachment.FileSize,
                Description = attachment.Description,
                UploadedById = attachment.UploadedById,
                UploadedAt = attachment.UploadedAt,
                DownloadUrl = $"/api/tasks/{taskId}/attachments/{attachment.Id}/download"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file for task {TaskId}", taskId);
            return StatusCode(500, new { error = "Failed to upload file" });
        }
    }

    /// <summary>
    /// Download attachment
    /// </summary>
    [HttpGet("{attachmentId}/download")]
    public async Task<IActionResult> DownloadAttachment(int taskId, int attachmentId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var attachment = await _context.TaskAttachments
            .Include(a => a.Task)
            .FirstOrDefaultAsync(a => a.Id == attachmentId && a.TaskId == taskId);

        if (attachment == null)
        {
            return NotFound(new { error = "Attachment not found" });
        }

        // Check if user owns the task
        if (attachment.Task?.UserId != userId)
        {
            return Forbid();
        }

        try
        {
            var stream = await _fileStorage.GetFileAsync(attachment.StoragePath);
            return File(stream, attachment.ContentType, attachment.OriginalFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to download attachment {AttachmentId}", attachmentId);
            return NotFound(new { error = "File not found" });
        }
    }

    /// <summary>
    /// Delete attachment
    /// </summary>
    [HttpDelete("{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment(int taskId, int attachmentId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var attachment = await _context.TaskAttachments
            .Include(a => a.Task)
            .FirstOrDefaultAsync(a => a.Id == attachmentId && a.TaskId == taskId);

        if (attachment == null)
        {
            return NotFound(new { error = "Attachment not found" });
        }

        // Check if user owns the task
        if (attachment.Task?.UserId != userId)
        {
            return Forbid();
        }

        try
        {
            await _fileStorage.DeleteFileAsync(attachment.StoragePath);
            _context.TaskAttachments.Remove(attachment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Attachment {AttachmentId} deleted from task {TaskId}", attachmentId, taskId);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment {AttachmentId}", attachmentId);
            return StatusCode(500, new { error = "Failed to delete attachment" });
        }
    }
}

