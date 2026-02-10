using GradLink.Application.DTOs.Materials;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for managing personal notes on materials
/// </summary>
[ApiController]
[Route("api/materials/{materialId}/notes")]
[Produces("application/json")]
[Authorize]
public class MaterialNotesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MaterialNotesController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get all notes for a material (user's own notes only)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<MaterialNoteDto>>> GetNotes(int materialId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notes = await _context.MaterialNotes
            .Where(n => n.MaterialId == materialId && n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return Ok(notes.Select(n => new MaterialNoteDto
        {
            Id = n.Id,
            MaterialId = n.MaterialId,
            Content = n.Content,
            Position = n.Position,
            Color = n.Color,
            CreatedAt = n.CreatedAt,
            UpdatedAt = n.UpdatedAt
        }));
    }

    /// <summary>
    /// Add a new note to a material
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MaterialNoteDto>> AddNote(int materialId, [FromBody] CreateNoteRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var material = await _context.Materials.FindAsync(materialId);
        if (material == null)
            return NotFound(new { message = "Material not found" });

        var note = new MaterialNote
        {
            MaterialId = materialId,
            UserId = userId,
            Content = request.Content,
            Position = request.Position,
            Color = request.Color,
            CreatedAt = DateTime.UtcNow
        };

        _context.MaterialNotes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNotes), new { materialId }, new MaterialNoteDto
        {
            Id = note.Id,
            MaterialId = note.MaterialId,
            Content = note.Content,
            Position = note.Position,
            Color = note.Color,
            CreatedAt = note.CreatedAt
        });
    }

    /// <summary>
    /// Update a note
    /// </summary>
    [HttpPut("{noteId}")]
    public async Task<ActionResult<MaterialNoteDto>> UpdateNote(int materialId, int noteId, [FromBody] UpdateNoteRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var note = await _context.MaterialNotes.FindAsync(noteId);
        if (note == null || note.MaterialId != materialId)
            return NotFound(new { message = "Note not found" });

        if (note.UserId != userId)
            return Forbid();

        if (!string.IsNullOrEmpty(request.Content))
            note.Content = request.Content;

        if (request.Position != null)
            note.Position = request.Position;

        if (request.Color != null)
            note.Color = request.Color;

        note.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new MaterialNoteDto
        {
            Id = note.Id,
            MaterialId = note.MaterialId,
            Content = note.Content,
            Position = note.Position,
            Color = note.Color,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        });
    }

    /// <summary>
    /// Delete a note
    /// </summary>
    [HttpDelete("{noteId}")]
    public async Task<IActionResult> DeleteNote(int materialId, int noteId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var note = await _context.MaterialNotes.FindAsync(noteId);
        if (note == null || note.MaterialId != materialId)
            return NotFound(new { message = "Note not found" });

        if (note.UserId != userId)
            return Forbid();

        _context.MaterialNotes.Remove(note);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Note deleted successfully" });
    }

    /// <summary>
    /// Get all notes across all materials for the user
    /// </summary>
    [HttpGet("/api/materials/my-notes")]
    public async Task<ActionResult<List<MaterialNoteDto>>> GetAllMyNotes()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notes = await _context.MaterialNotes
            .Where(n => n.UserId == userId)
            .Include(n => n.Material)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return Ok(notes.Select(n => new
        {
            n.Id,
            n.MaterialId,
            MaterialTitle = n.Material.Title,
            MaterialType = n.Material.Type,
            n.Content,
            n.Position,
            n.Color,
            n.CreatedAt,
            n.UpdatedAt
        }));
    }
}















