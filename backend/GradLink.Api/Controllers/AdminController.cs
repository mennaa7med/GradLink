using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public async Task<ActionResult> GetStats()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalProjects = await _context.Projects.CountAsync();
        var totalResumes = await _context.Resumes.CountAsync();
        var totalJobs = await _context.JobPostings.CountAsync();
        var totalMatches = await _context.Matches.CountAsync();
        var totalConversations = await _context.Conversations.CountAsync();

        var recentUsers = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(5)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FullName,
                u.CreatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            totalUsers,
            totalProjects,
            totalResumes,
            totalJobs,
            totalMatches,
            totalConversations,
            recentUsers
        });
    }

    [HttpGet("users")]
    public async Task<ActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _context.Users.AsQueryable();

        var total = await query.CountAsync();
        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FullName,
                u.Major,
                u.Company,
                u.CreatedAt,
                u.LastLoginAt
            })
            .ToListAsync();

        return Ok(new
        {
            total,
            page,
            pageSize,
            users
        });
    }
}

