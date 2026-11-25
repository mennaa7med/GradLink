using System.Text.RegularExpressions;

namespace GradLink.Api;

/// <summary>
/// Optional demo-only deployment mode. Off unless configuration "Demo:Enabled" is true.
/// When on, the API fails closed: only the demo session endpoint and a short list of
/// read-only (GET/HEAD/OPTIONS) exact routes are served. Everything else returns 403.
/// This closes registration, every login route, all writes, SignalR, chat proxy and payments.
/// </summary>
public sealed class DemoModeMiddleware
{
    // Exact, individually vetted GET routes. Unknown subroutes are denied.
    // Each pattern is matched against the whole path (case-insensitive, optional trailing slash).
    private static readonly Regex[] AllowedGetPatterns = new[]
    {
        // Materials (public seeded catalogue, plus owner-scoped stats/recommendations for the demo user)
        @"/api/materials", @"/api/materials/\d+", @"/api/materials/stats", @"/api/materials/top-rated",
        @"/api/materials/featured", @"/api/materials/my-stats", @"/api/materials/recommendations",
        // Mentors and sponsors (public lists)
        @"/api/mentors", @"/api/mentors/\d+", @"/api/mentors/search",
        @"/api/mentors/specialization/[^/]+",
        @"/api/sponsors", @"/api/sponsors/\d+", @"/api/sponsors/stats",
        // Internships and career (public lists)
        @"/api/internships", @"/api/internships/\d+",
        @"/api/career", @"/api/career/jobs", @"/api/career/projects", @"/api/career/stats",
        // Owner-scoped dashboard data (filtered by the caller's own id in each handler)
        @"/api/projects", @"/api/projects/\d+", @"/api/projects/\d+/tasks",
        @"/api/tasks", @"/api/tasks/\d+", @"/api/tasks/stats", @"/api/tasks/people",
        @"/api/notifications", @"/api/notifications/count",
        @"/api/users/me"
    }.Select(x => new Regex("^" + x + "/?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100))).ToArray();

    public const string DemoSessionPath = "/api/demo/session";

    private readonly RequestDelegate _next;
    private readonly bool _enabled;

    public DemoModeMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _enabled = configuration.GetValue<bool>("Demo:Enabled");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_enabled || IsAllowed(context.Request.Method, context.Request.Path))
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\":\"This action is disabled in the read-only demo.\"}");
    }

    public static bool IsAllowed(string method, PathString path)
    {
        if (HttpMethods.IsOptions(method))
        {
            return true; // CORS preflight
        }

        if (HttpMethods.IsPost(method) && path.Equals(DemoSessionPath, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method))
        {
            var value = path.Value ?? string.Empty;
            foreach (var pattern in AllowedGetPatterns)
            {
                if (pattern.IsMatch(value))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
