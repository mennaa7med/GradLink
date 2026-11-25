using FluentAssertions;
using GradLink.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GradLink.Tests;

public class DemoModeMiddlewareTests
{
    private static async Task<HttpClient> CreateClientAsync(bool? enabled)
    {
        var settings = new Dictionary<string, string?>();
        if (enabled.HasValue)
        {
            settings["Demo:Enabled"] = enabled.Value.ToString();
        }

        var host = await new HostBuilder()
            .ConfigureWebHost(web =>
            {
                web.UseTestServer();
                web.ConfigureAppConfiguration(c => c.AddInMemoryCollection(settings));
                web.Configure(app =>
                {
                    app.UseMiddleware<DemoModeMiddleware>();
                    app.Run(ctx =>
                    {
                        ctx.Response.StatusCode = 200;
                        return ctx.Response.WriteAsync("ok");
                    });
                });
            })
            .StartAsync();
        return host.GetTestClient();
    }

    private static HttpRequestMessage Req(string method, string path) => new(new HttpMethod(method), path);

    [Theory]
    [InlineData("GET", "/api/materials")]
    [InlineData("GET", "/api/materials/12")]
    [InlineData("HEAD", "/api/mentors")]
    [InlineData("GET", "/api/sponsors")]
    [InlineData("GET", "/api/internships")]
    [InlineData("GET", "/api/career")]
    [InlineData("GET", "/api/materials/featured")]
    [InlineData("GET", "/api/mentors/specialization/Frontend")]
    [InlineData("GET", "/api/career/stats")]
    [InlineData("GET", "/api/projects")]
    [InlineData("GET", "/api/projects/3")]
    [InlineData("GET", "/api/projects/3/tasks")]
    [InlineData("GET", "/api/tasks")]
    [InlineData("GET", "/api/tasks/stats")]
    [InlineData("GET", "/api/notifications/count")]
    [InlineData("GET", "/api/users/me")]
    [InlineData("OPTIONS", "/api/auth/login")]
    [InlineData("POST", "/api/demo/session")]
    public async Task DemoOn_AllowsOnlyAllowlist(string method, string path)
    {
        var client = await CreateClientAsync(true);
        var res = await client.SendAsync(Req(method, path));
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("POST", "/api/auth/register")]
    [InlineData("POST", "/api/auth/register/sponsor")]
    [InlineData("POST", "/api/auth/login")]
    [InlineData("POST", "/api/auth/refresh")]
    [InlineData("POST", "/api/auth/verify-email")]
    [InlineData("POST", "/api/auth/resend-otp")]
    [InlineData("POST", "/api/oauth/google")]
    [InlineData("POST", "/api/materials")]
    [InlineData("PUT", "/api/materials/12")]
    [InlineData("DELETE", "/api/materials/12")]
    [InlineData("GET", "/api/materialsX")]
    [InlineData("GET", "/api/users")]
    [InlineData("GET", "/api/jobs")]
    [InlineData("GET", "/api/projects/1/members")]
    [InlineData("GET", "/api/tasks/1/attachments")]
    [InlineData("GET", "/api/mentors/applications")]
    [InlineData("GET", "/api/sponsors/applications")]
    [InlineData("GET", "/api/sponsors/applications/me")]
    [InlineData("GET", "/api/materials/pending")]
    [InlineData("GET", "/api/users/abc")]
    [InlineData("GET", "/api/users/abc/public")]
    [InlineData("GET", "/api/career/company/1")]
    [InlineData("GET", "/api/internships/my")]
    [InlineData("GET", "/api/notifications/push/config")]
    [InlineData("POST", "/api/projects")]
    [InlineData("DELETE", "/api/tasks/1")]
    [InlineData("PUT", "/api/users/me")]
    [InlineData("GET", "/api/conversations")]
    [InlineData("GET", "/api/admin/users")]
    [InlineData("GET", "/hubs/chat")]
    [InlineData("POST", "/hubs/chat/negotiate")]
    [InlineData("POST", "/api/chatproxy/chat")]
    [InlineData("GET", "/api/chatproxy/test")]
    [InlineData("POST", "/api/payments/webhook")]
    [InlineData("POST", "/api/contact")]
    [InlineData("GET", "/api/demo/session")]
    [InlineData("POST", "/api/demo/session/x")]
    [InlineData("GET", "/api/materials/../users")]
    public async Task DemoOn_BlocksEverythingElse(string method, string path)
    {
        var client = await CreateClientAsync(true);
        var res = await client.SendAsync(Req(method, path));
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    public async Task DemoOffOrMissing_ChangesNothing(bool? enabled)
    {
        var client = await CreateClientAsync(enabled);
        foreach (var (m, p) in new[] { ("POST", "/api/auth/login"), ("POST", "/api/auth/register"), ("GET", "/hubs/chat"), ("PUT", "/api/materials/1") })
        {
            var res = await client.SendAsync(Req(m, p));
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
