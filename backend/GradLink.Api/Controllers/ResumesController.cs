using System.Text.Json;
using GradLink.Application.Common.Interfaces;
using GradLink.Application.DTOs.Resumes;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResumesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IAnalyzerService _analyzerService;
    private readonly IBackgroundQueue _backgroundQueue;
    private readonly ILogger<ResumesController> _logger;

    public ResumesController(
        AppDbContext context,
        IFileStorage fileStorage,
        IAnalyzerService analyzerService,
        IBackgroundQueue backgroundQueue,
        ILogger<ResumesController> logger)
    {
        _context = context;
        _fileStorage = fileStorage;
        _analyzerService = analyzerService;
        _backgroundQueue = backgroundQueue;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResumeDto>>> GetResumes()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var resumes = await _context.Resumes
            .Where(r => r.UserId == userId)
            .ToListAsync();

        var resumeDtos = resumes.Select(r => new ResumeDto
        {
            Id = r.Id,
            FileName = r.FileName,
            FileSize = r.FileSize,
            UploadedAt = r.UploadedAt,
            AnalysisStatus = r.AnalysisStatus,
            AnalyzedAt = r.AnalyzedAt,
            Skills = r.Skills != null ? JsonSerializer.Deserialize<List<string>>(r.Skills) : null,
            Education = r.Education,
            Experience = r.Experience,
            Summary = r.Summary,
            QualityScore = r.QualityScore,
            UserId = r.UserId
        }).ToList();

        return Ok(resumeDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResumeDto>> GetResume(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var resume = await _context.Resumes
            .Where(r => r.Id == id && r.UserId == userId)
            .FirstOrDefaultAsync();

        if (resume == null)
        {
            return NotFound();
        }

        var resumeDto = new ResumeDto
        {
            Id = resume.Id,
            FileName = resume.FileName,
            FileSize = resume.FileSize,
            UploadedAt = resume.UploadedAt,
            AnalysisStatus = resume.AnalysisStatus,
            AnalyzedAt = resume.AnalyzedAt,
            ExtractedText = resume.ExtractedText,
            Skills = resume.Skills != null ? JsonSerializer.Deserialize<List<string>>(resume.Skills) : null,
            Education = resume.Education,
            Experience = resume.Experience,
            Summary = resume.Summary,
            QualityScore = resume.QualityScore,
            UserId = resume.UserId
        };

        return Ok(resumeDto);
    }

    [HttpPost("upload")]
    public async Task<ActionResult<ResumeDto>> UploadResume(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "No file uploaded" });
        }

        var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest(new { error = "Invalid file type. Only PDF and Word documents are allowed." });
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        // Save file
        using var stream = file.OpenReadStream();
        var filePath = await _fileStorage.SaveFileAsync(stream, file.FileName, "resumes");

        // Create resume record
        var resume = new Resume
        {
            FileName = file.FileName,
            FilePath = filePath,
            FileSize = file.Length,
            UserId = userId,
            UploadedAt = DateTime.UtcNow,
            AnalysisStatus = "Pending"
        };

        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();

        // Queue analysis
        QueueResumeAnalysis(resume.Id);

        var resumeDto = new ResumeDto
        {
            Id = resume.Id,
            FileName = resume.FileName,
            FileSize = resume.FileSize,
            UploadedAt = resume.UploadedAt,
            AnalysisStatus = resume.AnalysisStatus,
            UserId = resume.UserId
        };

        return CreatedAtAction(nameof(GetResume), new { id = resume.Id }, resumeDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResume(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var resume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (resume == null)
        {
            return NotFound();
        }

        // Delete file
        try
        {
            await _fileStorage.DeleteFileAsync(resume.FilePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete file for resume {ResumeId}", id);
        }

        _context.Resumes.Remove(resume);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private void QueueResumeAnalysis(int resumeId)
    {
        _backgroundQueue.QueueBackgroundWorkItem(async cancellationToken =>
        {
            using var scope = HttpContext.RequestServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var fileStorage = scope.ServiceProvider.GetRequiredService<IFileStorage>();
            var analyzerService = scope.ServiceProvider.GetRequiredService<IAnalyzerService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResumesController>>();

            try
            {
                var resume = await context.Resumes.FindAsync(resumeId);
                if (resume == null) return;

                resume.AnalysisStatus = "Processing";
                await context.SaveChangesAsync(cancellationToken);

                // Get file path
                string fullPath;
                if (fileStorage is Infrastructure.Services.FileStorage.LocalFileStorage localStorage)
                {
                    fullPath = localStorage.GetFullPath(resume.FilePath);
                }
                else
                {
                    // For Azure Blob Storage, download to temp file
                    var tempPath = Path.GetTempFileName();
                    using (var fileStream = await fileStorage.GetFileAsync(resume.FilePath, cancellationToken))
                    using (var outputStream = System.IO.File.Create(tempPath))
                    {
                        await fileStream.CopyToAsync(outputStream, cancellationToken);
                    }
                    fullPath = tempPath;
                }

                // Analyze
                var result = await analyzerService.AnalyzeResumeAsync(fullPath, cancellationToken);

                // Update resume
                resume.AnalysisStatus = result.Success ? "Completed" : "Failed";
                resume.AnalyzedAt = DateTime.UtcNow;
                resume.ExtractedText = result.ExtractedText;
                resume.Skills = result.Skills != null ? JsonSerializer.Serialize(result.Skills) : null;
                resume.Education = result.Education;
                resume.Experience = result.Experience;
                resume.Summary = result.Summary;
                resume.QualityScore = result.QualityScore;

                await context.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Resume {ResumeId} analyzed successfully", resumeId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error analyzing resume {ResumeId}", resumeId);

                var resume = await context.Resumes.FindAsync(resumeId);
                if (resume != null)
                {
                    resume.AnalysisStatus = "Failed";
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        });
    }
}

