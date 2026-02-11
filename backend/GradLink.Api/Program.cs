using System.Text;
using AspNetCoreRateLimit;
using GradLink.Api.Hubs;
using GradLink.Application.Common.Interfaces;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Identity;
using GradLink.Infrastructure.Identity.Seeding;
using GradLink.Infrastructure.Persistence;
using GradLink.Infrastructure.Services;
using GradLink.Infrastructure.Services.FileStorage;
using GradLink.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Railway: Configure port from environment variable
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/gradlink-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

// Database
var connectionString = configuration.GetConnectionString("DefaultConnection");
var databaseProvider = configuration["DatabaseProvider"] ?? "Sqlite";

if (databaseProvider == "SqlServer")
{
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionString));
}

// Identity
services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT Configuration
var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions!.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        ClockSkew = TimeSpan.Zero
    };

    // SignalR support
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/chat"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

services.AddAuthorization();

// Application Services
services.AddScoped<JwtTokenService>();
services.AddScoped<DataSeeder>();
services.AddScoped<MaterialSeeder>();

// Background Services
services.AddSingleton<IBackgroundQueue, BackgroundQueue>();
services.AddHostedService<QueuedAnalysisService>();

// File Storage
var storageMode = configuration["FileStorage:Mode"] ?? "Local";
if (storageMode == "Azure")
{
    services.AddSingleton<IFileStorage, AzureBlobFileStorage>();
}
else
{
    services.AddSingleton<IFileStorage, LocalFileStorage>();
}

// Analyzer Service
services.AddHttpClient<IAnalyzerService, AnalyzerService>();

// HTTP Client Factory for OAuth
services.AddHttpClient();

// Email Service
services.AddScoped<IEmailService, EmailService>();

// Rate Limiting
services.AddMemoryCache();
services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
services.AddInMemoryRateLimiting();
services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// CORS - Configure allowed origins
services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        // Get origins from config, environment, or use defaults
        var configOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        
        // Always include these production URLs
        var defaultOrigins = new[] 
        { 
            "http://localhost:5173", 
            "http://localhost:5176",
            "http://localhost:3000", 
            "https://mennaa7med.github.io",
            "https://grad-link-x3ls.vercel.app",
            "https://gradlink-production-7fdd.up.railway.app"
        };
        
        var allOrigins = configOrigins.Concat(defaultOrigins).Distinct().ToArray();
        
        policy.WithOrigins(allOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    
    // Permissive policy for all environments (Railway/Production)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Controllers
services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// SignalR
services.AddSignalR();

// Swagger - Professional Configuration
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "🎓 GradLink API",
        Version = "v1.0.0",
        Description = "GradLink - Graduate Networking & Career Platform API"
    });

    // API Tags for grouping
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null) return new[] { api.GroupName };
        
        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return controllerName switch
        {
            "Auth" => new[] { "🔐 Authentication" },
            "Users" => new[] { "👤 Users" },
            "Jobs" => new[] { "💼 Jobs" },
            "Internships" => new[] { "📚 Internships" },
            "Projects" => new[] { "📁 Projects" },
            "ProjectTasks" => new[] { "✅ Project Tasks" },
            "Tasks" => new[] { "📋 Tasks" },
            "Career" => new[] { "🎯 Career (Public)" },
            "Applications" => new[] { "📝 Applications" },
            "Resumes" => new[] { "📄 Resumes" },
            "Notifications" => new[] { "🔔 Notifications" },
            "Conversations" => new[] { "💬 Conversations" },
            "Messages" => new[] { "✉️ Messages" },
            "Admin" => new[] { "🛡️ Admin" },
            "Mentor" => new[] { "👨‍🏫 Mentors" },
            "Sponsors" => new[] { "💰 Sponsors" },
            "ChatProxy" => new[] { "🤖 AI Assistant" },
            "SmartMatch" => new[] { "🎯 Smart Match" },
            "Analytics" => new[] { "📊 Analytics" },
            _ => new[] { controllerName ?? "Other" }
        };
    });

    c.DocInclusionPredicate((name, api) => true);

    // Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.

**How to authenticate:**
1. Login using `/api/auth/login`
2. Copy the `accessToken` from the response
3. Click **Authorize** button above
4. Enter: `Bearer {your_token}`

**Example:** `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();

        // Seed test questions for mentor applications
        await GradLink.Api.Data.TestQuestionSeeder.SeedTestQuestionsAsync(context);
        
        // Seed materials
        var materialSeeder = scope.ServiceProvider.GetRequiredService<MaterialSeeder>();
        await materialSeeder.SeedMaterialsAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline.
// Enable Swagger in all environments for Railway
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GradLink API v1");
    c.DocumentTitle = "GradLink API Documentation";
    c.DefaultModelsExpandDepth(2);
    c.DefaultModelExpandDepth(2);
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    c.EnableDeepLinking();
    c.DisplayRequestDuration();
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableTryItOutByDefault();
    
    // Custom CSS for professional look
    c.HeadContent = @"
        <style>
            .swagger-ui .topbar { display: none; }
            .swagger-ui .info { margin: 30px 0; }
            .swagger-ui .info .title { font-size: 36px; color: #3b4151; }
            .swagger-ui .info .description { font-size: 14px; }
            .swagger-ui .info .description p { margin: 10px 0; }
            .swagger-ui .opblock-tag { font-size: 18px !important; border-bottom: 2px solid #6366f1 !important; margin: 20px 0 10px 0 !important; padding-bottom: 10px !important; }
            .swagger-ui .opblock.opblock-get { background: rgba(97, 175, 254, 0.1); border-color: #61affe; }
            .swagger-ui .opblock.opblock-post { background: rgba(73, 204, 144, 0.1); border-color: #49cc90; }
            .swagger-ui .opblock.opblock-put { background: rgba(252, 161, 48, 0.1); border-color: #fca130; }
            .swagger-ui .opblock.opblock-delete { background: rgba(249, 62, 62, 0.1); border-color: #f93e3e; }
            .swagger-ui .btn.authorize { background-color: #6366f1; border-color: #6366f1; }
            .swagger-ui .btn.authorize svg { fill: #fff; }
            .swagger-ui .scheme-container { background: #f8fafc; padding: 20px; border-radius: 8px; }
            .swagger-ui .model-box { background: #f8fafc; }
            .swagger-ui section.models { border: 1px solid #e2e8f0; border-radius: 8px; }
            .swagger-ui section.models h4 { color: #3b4151; }
        </style>
    ";
});

// CRITICAL: Handle OPTIONS preflight FIRST - before anything else
app.Use(async (context, next) =>
{
    // Add CORS headers to ALL responses
    var origin = context.Request.Headers["Origin"].ToString();
    if (!string.IsNullOrEmpty(origin))
    {
        context.Response.Headers["Access-Control-Allow-Origin"] = origin;
        context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
    }
    
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, PATCH, OPTIONS, HEAD";
        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization, X-Requested-With, Accept, Origin";
        context.Response.Headers["Access-Control-Max-Age"] = "86400";
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    
    await next();
});

app.UseSerilogRequestLogging();

// CORS policy as backup
app.UseCors("AllowAll");

// Only redirect to HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

// Rate limiting after auth to avoid blocking preflight
app.UseIpRateLimiting();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();
