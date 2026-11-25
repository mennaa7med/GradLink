using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GradLink.Tests;

/// <summary>Runs the real app against a throwaway local SQLite file with demo mode on.</summary>
public class DemoEndToEndTests : IDisposable
{
    private readonly string _db = Path.Combine(Path.GetTempPath(), "gradlink-demo-test-" + Guid.NewGuid().ToString("N") + ".db");

    // Migrations target SQL Server. Build the schema from the model and mark every migration applied,
    // so the app's own MigrateAsync() is a no-op on this throwaway SQLite file. Test-only; the app is unchanged.
    private void PrepareSchema()
    {
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<GradLink.Infrastructure.Persistence.AppDbContext>()
            .UseSqlite("Data Source=" + _db).Options;
        using var ctx = new GradLink.Infrastructure.Persistence.AppDbContext(options);
        ctx.Database.EnsureCreated();
        ctx.Database.ExecuteSqlRaw("CREATE TABLE IF NOT EXISTS \"__EFMigrationsHistory\" (\"MigrationId\" TEXT NOT NULL PRIMARY KEY, \"ProductVersion\" TEXT NOT NULL)");
        foreach (var id in ctx.Database.GetMigrations())
        {
            ctx.Database.ExecuteSqlRaw("INSERT OR IGNORE INTO \"__EFMigrationsHistory\" VALUES ({0}, '8.0.0')", id);
        }
    }

    private WebApplicationFactory<Program> Create(bool demo)
    {
        PrepareSchema();
        return new WebApplicationFactory<Program>().WithWebHostBuilder(b =>
        {
            b.UseEnvironment("Development");
            b.UseSetting("DatabaseProvider", "Sqlite");
            b.UseSetting("ConnectionStrings:DefaultConnection", "Data Source=" + _db);
            b.UseSetting("Demo:Enabled", demo ? "true" : "false");
            b.UseSetting("Jwt:Secret", "test-only-secret-test-only-secret-test-only-secret-1234");
            b.UseSetting("JwtSettings:Secret", "test-only-secret-test-only-secret-test-only-secret-1234");
        });
    }

    [Fact]
    public async Task DemoOn_SessionWorks_AndOnlySyntheticReadOnlyAccess()
    {
        using var factory = Create(true);
        var client = factory.CreateClient();

        var anon = await client.GetAsync("/api/projects");
        anon.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var session = await client.PostAsync("/api/demo/session", null);
        session.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = JsonDocument.Parse(await session.Content.ReadAsStringAsync()).RootElement;
        var token = json.EnumerateObject().First(p => p.Name.Equals("accessToken", StringComparison.OrdinalIgnoreCase) || p.Name.Equals("token", StringComparison.OrdinalIgnoreCase)).Value.GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var projects = await client.GetStringAsync("/api/projects");
        projects.Should().Contain("Sample: Campus Events App");
        (await client.GetStringAsync("/api/tasks")).Should().Contain("Build the events API");
        (await client.GetStringAsync("/api/mentors")).Should().Contain("gradlink-demo.invalid");
        (await client.GetStringAsync("/api/users/me")).Should().Contain("demo.student@gradlink-demo.invalid");
        (await client.GetAsync("/api/materials")).StatusCode.Should().Be(HttpStatusCode.OK);

        // writes, other routes and logins are closed even with a valid demo token
        (await client.PostAsJsonAsync("/api/projects", new { title = "x" })).StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await client.DeleteAsync("/api/tasks/1")).StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await client.PostAsJsonAsync("/api/auth/login", new { email = "a@b.c", password = "x" })).StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await client.PostAsJsonAsync("/api/auth/register", new { email = "a@b.c", password = "x" })).StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await client.GetAsync("/api/admin/users")).StatusCode.Should().Be(HttpStatusCode.Forbidden);

        // demo session is repeatable and does not duplicate data
        (await client.PostAsync("/api/demo/session", null)).StatusCode.Should().Be(HttpStatusCode.OK);
        var again = await client.GetStringAsync("/api/projects");
        again.Split("Sample: Campus Events App").Length.Should().Be(2);
    }

    [Fact]
    public async Task DemoOff_SessionIs404_AndNoDemoDataSeeded()
    {
        using var factory = Create(false);
        var client = factory.CreateClient();
        (await client.PostAsync("/api/demo/session", null)).StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await client.GetStringAsync("/api/mentors")).Should().NotContain("gradlink-demo.invalid");
        (await client.PostAsJsonAsync("/api/auth/login", new { email = "a@b.c", password = "x" })).StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }

    public void Dispose() { try { File.Delete(_db); } catch { } }
}
