using System.Text.Json;
using GradLink.Application.DTOs.Career;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// 🎯 Public Career Page API - Displays all opportunities from all companies
/// </summary>
/// <remarks>
/// No authentication required for viewing public opportunities.
/// This endpoint provides data for the public career page, allowing users to browse
/// all available jobs, internships, and project-based opportunities.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CareerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CareerController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all opportunities grouped by company (Public endpoint)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CareerPageResponse>> GetAllOpportunities([FromQuery] CareerFilterDto? filter)
    {
        filter ??= new CareerFilterDto();

        // Get all active jobs with their company info
        var jobsQuery = _context.JobPostings
            .Where(j => j.Status == "Active")
            .Include(j => j.PostedBy)
            .AsQueryable();

        // Get all active projects with their owner info
        var projectsQuery = _context.Projects
            .Where(p => p.Status == "Active")
            .Include(p => p.Owner)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(filter.Search))
        {
            var searchLower = filter.Search.ToLower();
            jobsQuery = jobsQuery.Where(j => 
                j.Title.ToLower().Contains(searchLower) || 
                (j.Description != null && j.Description.ToLower().Contains(searchLower)) ||
                (j.CompanyName != null && j.CompanyName.ToLower().Contains(searchLower)));
            
            projectsQuery = projectsQuery.Where(p => 
                p.Title.ToLower().Contains(searchLower) || 
                (p.Description != null && p.Description.ToLower().Contains(searchLower)));
        }

        // Apply location filter
        if (!string.IsNullOrEmpty(filter.Location))
        {
            var locationLower = filter.Location.ToLower();
            jobsQuery = jobsQuery.Where(j => j.Location != null && j.Location.ToLower().Contains(locationLower));
        }

        // Apply employment type filter for jobs
        if (!string.IsNullOrEmpty(filter.EmploymentType))
        {
            jobsQuery = jobsQuery.Where(j => j.EmploymentType == filter.EmploymentType);
        }

        // Get all active internships
        var internshipsQuery = _context.Internships
            .Where(i => i.Status == "Active")
            .Include(i => i.PostedBy)
            .AsQueryable();

        // Apply search filter to internships
        if (!string.IsNullOrEmpty(filter.Search))
        {
            var searchLower = filter.Search.ToLower();
            internshipsQuery = internshipsQuery.Where(i =>
                i.Title.ToLower().Contains(searchLower) ||
                (i.Description != null && i.Description.ToLower().Contains(searchLower)) ||
                (i.CompanyName != null && i.CompanyName.ToLower().Contains(searchLower)));
        }

        // Apply location filter to internships
        if (!string.IsNullOrEmpty(filter.Location))
        {
            var locationLower = filter.Location.ToLower();
            internshipsQuery = internshipsQuery.Where(i => i.Location != null && i.Location.ToLower().Contains(locationLower));
        }

        // Fetch data
        var jobs = await jobsQuery.ToListAsync();
        var projects = await projectsQuery.ToListAsync();
        var internships = await internshipsQuery.ToListAsync();

        // Group by company (using PostedById/OwnerId as company identifier)
        var companyGroups = new Dictionary<string, CompanyOpportunitiesDto>();

        // Process jobs
        foreach (var job in jobs)
        {
            var companyId = job.PostedById;
            if (!companyGroups.ContainsKey(companyId))
            {
                companyGroups[companyId] = new CompanyOpportunitiesDto
                {
                    CompanyId = companyId,
                    CompanyName = job.CompanyName ?? job.PostedBy?.CompanyName ?? job.PostedBy?.FullName ?? "Unknown Company",
                    CompanyLogo = job.PostedBy?.CompanyLogo,
                    Industry = job.PostedBy?.Industry,
                    Location = job.PostedBy?.Location ?? job.Location,
                    Website = job.PostedBy?.CompanyWebsite,
                    Description = job.PostedBy?.CompanyDescription,
                    IsVerified = false // Can be enhanced with CompanyProfile verification
                };
            }

            companyGroups[companyId].Jobs.Add(new OpportunityDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Skills = job.Skills != null ? JsonSerializer.Deserialize<List<string>>(job.Skills) : null,
                Requirements = job.Requirements != null ? JsonSerializer.Deserialize<List<string>>(job.Requirements) : null,
                Type = "Job",
                EmploymentType = job.EmploymentType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                Status = job.Status,
                CreatedAt = job.CreatedAt,
                ExpiresAt = job.ExpiresAt
            });
        }

        // Process internships
        foreach (var internship in internships)
        {
            var companyId = internship.PostedById;
            if (!companyGroups.ContainsKey(companyId))
            {
                companyGroups[companyId] = new CompanyOpportunitiesDto
                {
                    CompanyId = companyId,
                    CompanyName = internship.CompanyName ?? internship.PostedBy?.CompanyName ?? internship.PostedBy?.FullName ?? "Unknown Company",
                    CompanyLogo = internship.PostedBy?.CompanyLogo,
                    Industry = internship.PostedBy?.Industry,
                    Location = internship.PostedBy?.Location ?? internship.Location,
                    Website = internship.PostedBy?.CompanyWebsite,
                    Description = internship.PostedBy?.CompanyDescription,
                    IsVerified = false
                };
            }

            companyGroups[companyId].Internships.Add(new OpportunityDto
            {
                Id = internship.Id,
                Title = internship.Title,
                Description = internship.Description,
                Location = internship.Location,
                Type = "Internship",
                Duration = internship.Duration,
                IsPaid = internship.IsPaid,
                Status = internship.Status,
                CreatedAt = internship.CreatedAt,
                ExpiresAt = internship.ExpiresAt
            });
        }

        // Process projects (as project-based opportunities)
        foreach (var project in projects)
        {
            var companyId = project.OwnerId;
            if (!companyGroups.ContainsKey(companyId))
            {
                companyGroups[companyId] = new CompanyOpportunitiesDto
                {
                    CompanyId = companyId,
                    CompanyName = project.Owner?.CompanyName ?? project.Owner?.FullName ?? "Unknown Company",
                    CompanyLogo = project.Owner?.CompanyLogo,
                    Industry = project.Owner?.Industry,
                    Location = project.Owner?.Location,
                    Website = project.Owner?.CompanyWebsite,
                    Description = project.Owner?.CompanyDescription,
                    IsVerified = false
                };
            }

            companyGroups[companyId].Projects.Add(new OpportunityDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Type = "Project",
                Status = project.Status ?? "Active",
                CreatedAt = project.CreatedAt
            });
        }

        // Filter by opportunity type if specified
        if (!string.IsNullOrEmpty(filter.Type))
        {
            foreach (var company in companyGroups.Values)
            {
                if (filter.Type == "Job")
                {
                    company.Internships.Clear();
                    company.Projects.Clear();
                }
                else if (filter.Type == "Internship")
                {
                    company.Jobs.Clear();
                    company.Projects.Clear();
                }
                else if (filter.Type == "Project")
                {
                    company.Jobs.Clear();
                    company.Internships.Clear();
                }
            }
        }

        // Remove companies with no opportunities after filtering
        var companiesWithOpportunities = companyGroups.Values
            .Where(c => c.TotalOpportunities > 0)
            .OrderByDescending(c => c.TotalOpportunities)
            .ThenBy(c => c.CompanyName)
            .ToList();

        // Apply pagination
        var page = filter.Page ?? 1;
        var pageSize = filter.PageSize ?? 20;
        var pagedCompanies = companiesWithOpportunities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(new CareerPageResponse
        {
            Companies = pagedCompanies
        });
    }

    /// <summary>
    /// Get all jobs only (Public endpoint)
    /// </summary>
    [HttpGet("jobs")]
    public async Task<ActionResult<IEnumerable<OpportunityDto>>> GetAllJobs([FromQuery] string? search, [FromQuery] string? location)
    {
        var query = _context.JobPostings
            .Where(j => j.Status == "Active")
            .Include(j => j.PostedBy)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(j => 
                j.Title.ToLower().Contains(searchLower) || 
                (j.Description != null && j.Description.ToLower().Contains(searchLower)));
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(j => j.Location != null && j.Location.ToLower().Contains(location.ToLower()));
        }

        var jobs = await query.OrderByDescending(j => j.CreatedAt).ToListAsync();

        var result = jobs.Select(job => new OpportunityDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Location = job.Location,
            Skills = job.Skills != null ? JsonSerializer.Deserialize<List<string>>(job.Skills) : null,
            Requirements = job.Requirements != null ? JsonSerializer.Deserialize<List<string>>(job.Requirements) : null,
            Type = "Job",
            EmploymentType = job.EmploymentType,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            Status = job.Status,
            CreatedAt = job.CreatedAt,
            ExpiresAt = job.ExpiresAt
        });

        return Ok(result);
    }

    /// <summary>
    /// Get all projects only (Public endpoint)
    /// </summary>
    [HttpGet("projects")]
    public async Task<ActionResult<IEnumerable<OpportunityDto>>> GetAllProjects([FromQuery] string? search)
    {
        var query = _context.Projects
            .Where(p => p.Status == "Active")
            .Include(p => p.Owner)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(p => 
                p.Title.ToLower().Contains(searchLower) || 
                (p.Description != null && p.Description.ToLower().Contains(searchLower)));
        }

        var projects = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();

        var result = projects.Select(p => new OpportunityDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Type = "Project",
            Status = p.Status ?? "Active",
            CreatedAt = p.CreatedAt
        });

        return Ok(result);
    }

    /// <summary>
    /// Get opportunities for a specific company (Public endpoint)
    /// </summary>
    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<CompanyOpportunitiesDto>> GetCompanyOpportunities(string companyId)
    {
        // Get company user info
        var companyUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == companyId);
        
        if (companyUser == null)
        {
            return NotFound("Company not found");
        }

        // Get company profile if exists
        var companyProfile = await _context.CompanyProfiles
            .FirstOrDefaultAsync(cp => cp.UserId == companyId);

        var result = new CompanyOpportunitiesDto
        {
            CompanyId = companyId,
            CompanyName = companyProfile?.CompanyName ?? companyUser.CompanyName ?? companyUser.FullName ?? "Unknown Company",
            CompanyLogo = companyProfile?.LogoUrl ?? companyUser.CompanyLogo,
            Industry = companyProfile?.Industry ?? companyUser.Industry,
            Location = companyProfile?.Address ?? companyUser.Location,
            Website = companyProfile?.Website ?? companyUser.CompanyWebsite,
            Description = companyProfile?.CompanyDescription ?? companyUser.CompanyDescription,
            IsVerified = companyProfile?.IsVerified ?? false
        };

        // Get jobs
        var jobs = await _context.JobPostings
            .Where(j => j.PostedById == companyId && j.Status == "Active")
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        result.Jobs = jobs.Select(job => new OpportunityDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Location = job.Location,
            Skills = job.Skills != null ? JsonSerializer.Deserialize<List<string>>(job.Skills) : null,
            Requirements = job.Requirements != null ? JsonSerializer.Deserialize<List<string>>(job.Requirements) : null,
            Type = "Job",
            EmploymentType = job.EmploymentType,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            Status = job.Status,
            CreatedAt = job.CreatedAt,
            ExpiresAt = job.ExpiresAt
        }).ToList();

        // Get projects
        var projects = await _context.Projects
            .Where(p => p.OwnerId == companyId && p.Status == "Active")
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        result.Projects = projects.Select(p => new OpportunityDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Type = "Project",
            Status = p.Status ?? "Active",
            CreatedAt = p.CreatedAt
        }).ToList();

        return Ok(result);
    }

    /// <summary>
    /// Get career page statistics (Public endpoint)
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetCareerStats()
    {
        var totalJobs = await _context.JobPostings.CountAsync(j => j.Status == "Active");
        var totalProjects = await _context.Projects.CountAsync(p => p.Status == "Active");
        
        // Count unique companies (users who have posted jobs or projects)
        var companiesWithJobs = await _context.JobPostings
            .Where(j => j.Status == "Active")
            .Select(j => j.PostedById)
            .Distinct()
            .CountAsync();

        var companiesWithProjects = await _context.Projects
            .Where(p => p.Status == "Active")
            .Select(p => p.OwnerId)
            .Distinct()
            .CountAsync();

        var totalInternships = await _context.Internships.CountAsync(i => i.Status == "Active");

        return Ok(new
        {
            TotalJobs = totalJobs,
            TotalInternships = totalInternships,
            TotalProjects = totalProjects,
            TotalOpportunities = totalJobs + totalInternships + totalProjects,
            TotalCompanies = Math.Max(companiesWithJobs, companiesWithProjects)
        });
    }
}


