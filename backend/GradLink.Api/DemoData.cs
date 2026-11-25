using GradLink.Domain.Entities;
using GradLink.Infrastructure.Identity;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api;

/// <summary>
/// Synthetic data and demo user for the optional read-only demo mode (Demo:Enabled=true).
/// Every record is fake: example people, "@gradlink-demo.invalid" addresses, no real contacts.
/// Never called unless demo mode is on.
/// </summary>
public static class DemoData
{
    public const string DemoEmail = "demo.student@gradlink-demo.invalid";
    private const string FakeDomain = "@gradlink-demo.invalid";

    public static async Task<ApplicationUser?> GetOrCreateDemoUserAsync(UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByEmailAsync(DemoEmail);
        if (user != null)
        {
            return user;
        }

        user = new ApplicationUser
        {
            UserName = DemoEmail,
            Email = DemoEmail,
            FullName = "Demo Student",
            EmailConfirmed = true
        };

        // Random password that is never stored or shown; nobody can log in with it.
        var randomPassword = "Aa1!" + Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        var created = await userManager.CreateAsync(user, randomPassword);
        if (!created.Succeeded)
        {
            return null;
        }

        await userManager.AddToRoleAsync(user, "Student");
        return user;
    }

    public static async Task EnsureSeededAsync(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        var demoUser = await GetOrCreateDemoUserAsync(userManager);
        if (demoUser == null)
        {
            return;
        }

        if (!await context.Mentors.AnyAsync())
        {
            context.Mentors.AddRange(
                new Mentor { FullName = "Sara Example", Email = "sara.mentor" + FakeDomain, JobTitle = "Senior Frontend Engineer (sample)", Specialization = "Frontend", Bio = "Sample mentor profile for the read-only demo.", Skills = "React, JavaScript, CSS", ExperienceYears = 8 },
                new Mentor { FullName = "Omar Sample", Email = "omar.mentor" + FakeDomain, JobTitle = "Backend Engineer (sample)", Specialization = "Backend", Bio = "Sample mentor profile for the read-only demo.", Skills = "C#, ASP.NET Core, SQL", ExperienceYears = 6 },
                new Mentor { FullName = "Lina Demo", Email = "lina.mentor" + FakeDomain, JobTitle = "Data Analyst (sample)", Specialization = "Data", Bio = "Sample mentor profile for the read-only demo.", Skills = "Python, SQL, Power BI", ExperienceYears = 5 });
        }

        if (!await context.Sponsors.AnyAsync())
        {
            context.Sponsors.AddRange(
                new Sponsor { Name = "Example Tech Foundation", Field = "Education", SupportedProject = "Sample student platform grants" },
                new Sponsor { Name = "Sample Software Co.", Field = "Software Development", SupportedProject = "Sample open-source mentoring" });
        }

        if (!await context.Projects.AnyAsync(p => p.OwnerId == demoUser.Id))
        {
            var project = new Project
            {
                Title = "Sample: Campus Events App",
                Description = "A sample student project used only in the read-only demo.",
                Category = "Web",
                Status = "Active",
                OwnerId = demoUser.Id
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            context.Tasks.AddRange(
                new TaskItem { Name = "Design the home screen", UserId = demoUser.Id, ProjectId = project.Id, Category = "Frontend", Priority = "High", Status = "Completed", Person = "Demo Student" },
                new TaskItem { Name = "Build the events API", UserId = demoUser.Id, ProjectId = project.Id, Category = "Backend", Priority = "High", Status = "In Progress", Person = "Demo Student" },
                new TaskItem { Name = "Write the project presentation", UserId = demoUser.Id, ProjectId = project.Id, Category = "Presentation", Priority = "Medium", Status = "Pending", Person = "Demo Student" });
        }

        await context.SaveChangesAsync();
    }
}
