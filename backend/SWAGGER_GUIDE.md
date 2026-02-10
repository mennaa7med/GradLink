# Swagger API Documentation Guide

## ✅ Swagger is Already Configured!

Your GradLink backend has Swagger/OpenAPI fully configured and ready to use.

## 🚀 How to Access Swagger UI

### Step 1: Start the Backend

```bash
cd backend/GradLink.Api
dotnet run
```

Wait for the server to start:
```
info: Now listening on: https://localhost:5001
info: Now listening on: http://localhost:5000
```

### Step 2: Open Swagger UI

Open your browser and navigate to one of these URLs:

**HTTP (Recommended for development):**
```
http://localhost:5000/swagger
```

**HTTPS:**
```
https://localhost:5001/swagger
```

You should see the **Swagger UI** with all your API endpoints! 🎉

## 📚 What's Included in Swagger

### Current Configuration

✅ **Package:** Swashbuckle.AspNetCore v6.9.0
✅ **API Title:** GradLink API
✅ **Version:** v1
✅ **Description:** GradLink - Graduate Networking and Career Platform API
✅ **Authentication:** JWT Bearer Token support
✅ **All Controllers:** All 10 controllers with 40+ endpoints documented

## 🔐 Testing Authenticated Endpoints

Many endpoints require authentication. Here's how to test them:

### Step 1: Login to Get Token

1. Find the **POST /api/auth/login** endpoint
2. Click **"Try it out"**
3. Enter credentials:
   ```json
   {
     "email": "admin@gradlink.com",
     "password": "Admin@123"
   }
   ```
4. Click **"Execute"**
5. Copy the `accessToken` from the response

### Step 2: Authorize Swagger

1. Click the **"Authorize"** button (🔓 icon) at the top right
2. In the dialog, enter:
   ```
   Bearer YOUR_ACCESS_TOKEN_HERE
   ```
   ⚠️ **Important:** Include the word "Bearer" followed by a space, then your token
3. Click **"Authorize"**
4. Click **"Close"**

Now you can test all protected endpoints! The lock icons (🔒) will show as unlocked.

### Step 3: Test Endpoints

Try any endpoint:
1. Click on an endpoint to expand it
2. Click **"Try it out"**
3. Fill in parameters if needed
4. Click **"Execute"**
5. View the response below

## 📋 Available Endpoint Groups

### 1. Authentication (/api/auth)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get tokens
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - Logout (requires auth)

### 2. Users (/api/users)
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/me` - Get current user profile
- `PUT /api/users/me` - Update profile
- `POST /api/users/me/avatar` - Upload avatar

### 3. Projects (/api/projects)
- `GET /api/projects` - Get user's projects
- `GET /api/projects/{id}` - Get project details
- `POST /api/projects` - Create project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### 4. Project Tasks (/api/projects/{projectId}/tasks)
- `GET /api/projects/{projectId}/tasks` - Get project tasks
- `POST /api/projects/{projectId}/tasks` - Create task
- `PUT /api/projects/{projectId}/tasks/{taskId}` - Update task
- `DELETE /api/projects/{projectId}/tasks/{taskId}` - Delete task

### 5. Resumes (/api/resumes)
- `GET /api/resumes` - Get user's resumes
- `GET /api/resumes/{id}` - Get resume details
- `POST /api/resumes/upload` - Upload resume
- `DELETE /api/resumes/{id}` - Delete resume

### 6. Jobs (/api/jobs)
- `GET /api/jobs` - Get all active jobs
- `GET /api/jobs/my` - Get my job postings
- `GET /api/jobs/{id}` - Get job details
- `POST /api/jobs` - Create job posting
- `PUT /api/jobs/{id}` - Update job
- `DELETE /api/jobs/{id}` - Delete job

### 7. Matches (/api/matches)
- `GET /api/matches/resume/{resumeId}` - Get matches for resume
- `GET /api/matches/job/{jobId}` - Get candidates for job
- `POST /api/matches/run/{resumeId}` - Run matching algorithm

### 8. Conversations (/api/conversations)
- `GET /api/conversations` - Get user's conversations
- `GET /api/conversations/{id}` - Get conversation details
- `POST /api/conversations` - Create/get conversation

### 9. Admin (/api/admin)
- `GET /api/admin/stats` - Get platform statistics (Admin only)
- `GET /api/admin/users` - Get all users with pagination (Admin only)

### 10. Chat Proxy (/api/chatproxy)
- `POST /api/chatproxy/gemini` - Proxy to Gemini AI

## 🎯 Quick Test Workflow

### Test 1: Register a User
```
POST /api/auth/register
{
  "email": "test@example.com",
  "password": "Test@123",
  "fullName": "Test User",
  "phoneNumber": "+1234567890"
}
```

### Test 2: Login
```
POST /api/auth/login
{
  "email": "test@example.com",
  "password": "Test@123"
}
```
Copy the `accessToken` → Authorize

### Test 3: Get Profile
```
GET /api/users/me
```

### Test 4: Create a Project
```
POST /api/projects
{
  "title": "My First Project",
  "description": "Testing the API",
  "category": "Web Development"
}
```

## 📦 Swagger Configuration Details

### Location in Code

**File:** `backend/GradLink.Api/Program.cs`

**Lines 152-185:**
```csharp
// Swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GradLink API",
        Version = "v1",
        Description = "GradLink - Graduate Networking and Career Platform API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
});
```

### Middleware Configuration

**Lines 207-214:**
```csharp
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GradLink API v1");
    });
}
```

## 🔧 Customization Options

### Change API Title/Description

Edit `backend/GradLink.Api/Program.cs`:

```csharp
c.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Your Custom Title",
    Version = "v2.0",
    Description = "Your custom description",
    Contact = new OpenApiContact
    {
        Name = "Your Name",
        Email = "your.email@example.com",
        Url = new Uri("https://yourwebsite.com")
    }
});
```

### Enable in Production

By default, Swagger only runs in Development. To enable in production:

**Change:**
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

**To:**
```csharp
// Enable in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GradLink API v1");
});
```

⚠️ **Security Note:** Only enable Swagger in production if behind authentication!

### Add XML Documentation Comments

For detailed endpoint descriptions:

**Step 1:** Enable XML documentation in `GradLink.Api.csproj`:
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

**Step 2:** Add XML comments to controllers:
```csharp
/// <summary>
/// Get user profile by ID
/// </summary>
/// <param name="id">User ID</param>
/// <returns>User profile</returns>
/// <response code="200">Returns the user profile</response>
/// <response code="404">User not found</response>
[HttpGet("{id}")]
public async Task<ActionResult> GetUser(string id)
{
    // ...
}
```

**Step 3:** Configure Swagger to use XML:
```csharp
c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "GradLink.Api.xml"));
```

### Add API Versioning

```csharp
// In Program.cs
services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

c.SwaggerDoc("v1", new OpenApiInfo { Title = "GradLink API", Version = "v1" });
c.SwaggerDoc("v2", new OpenApiInfo { Title = "GradLink API", Version = "v2" });
```

## 🌐 Access from Different Machines

### From Same Network

If you want to access Swagger from another computer on your network:

**Step 1:** Find your IP address
```powershell
ipconfig
# Look for IPv4 Address (e.g., 192.168.1.100)
```

**Step 2:** Update `launchSettings.json`
```json
"applicationUrl": "http://0.0.0.0:5000;https://0.0.0.0:5001"
```

**Step 3:** Allow through firewall
```powershell
New-NetFirewallRule -DisplayName "GradLink API" -Direction Inbound -LocalPort 5000 -Protocol TCP -Action Allow
```

**Step 4:** Access from other machine
```
http://192.168.1.100:5000/swagger
```

## 📤 Export Swagger Documentation

### Export as JSON

```
http://localhost:5000/swagger/v1/swagger.json
```

Save this file to use with:
- Postman (Import → Upload Files)
- API testing tools
- Code generators

### Export as YAML

Add package:
```bash
dotnet add package Swashbuckle.AspNetCore.SwaggerGen
```

Then access:
```
http://localhost:5000/swagger/v1/swagger.yaml
```

## 🧪 Testing Tools Integration

### Postman

1. Open Postman
2. Import → Link
3. Enter: `http://localhost:5000/swagger/v1/swagger.json`
4. Click "Import"
5. All endpoints are now in Postman!

### Insomnia

1. Open Insomnia
2. Create → Import From → URL
3. Enter: `http://localhost:5000/swagger/v1/swagger.json`
4. Import

### VS Code REST Client

Create `.http` file:
```http
### Variables
@baseUrl = http://localhost:5000
@token = YOUR_ACCESS_TOKEN

### Login
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "admin@gradlink.com",
  "password": "Admin@123"
}

### Get Profile
GET {{baseUrl}}/api/users/me
Authorization: Bearer {{token}}
```

## 🎨 Swagger UI Themes

Customize the look:

```csharp
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GradLink API v1");
    c.RoutePrefix = string.Empty; // Swagger at root URL
    c.DocumentTitle = "GradLink API Documentation";
    c.DefaultModelsExpandDepth(-1); // Hide models by default
    c.DocExpansion(DocExpansion.None); // Collapse all by default
    c.InjectStylesheet("/swagger-custom.css"); // Custom CSS
});
```

## 🔍 Troubleshooting

### Issue: Swagger page not loading

**Solution 1:** Ensure you're in Development mode
```bash
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run
```

**Solution 2:** Check Program.cs has:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### Issue: "Failed to load API definition"

**Solution:** Check backend console for errors. Usually a serialization issue in DTOs.

### Issue: Authorization not working

**Solution:** Make sure to include "Bearer " prefix:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Issue: Some endpoints not showing

**Solution:** Ensure controllers have `[ApiController]` and `[Route]` attributes.

## ✅ Verification Checklist

- [ ] Backend is running
- [ ] Can access `http://localhost:5000/swagger`
- [ ] See "GradLink API" title at the top
- [ ] See all controller groups (Auth, Users, Projects, etc.)
- [ ] Can login and get a token
- [ ] Can authorize with token (🔓 becomes 🔒)
- [ ] Can test protected endpoints
- [ ] Responses show correctly

## 📚 Additional Resources

- [Swashbuckle Documentation](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [OpenAPI Specification](https://swagger.io/specification/)
- [Swagger UI Configuration](https://swagger.io/tools/swagger-ui/configuration/)

## 🎉 Summary

Your Swagger is **fully configured and ready to use**!

**Quick Access:**
```
1. cd backend/GradLink.Api
2. dotnet run
3. Open: http://localhost:5000/swagger
4. Login with admin credentials
5. Click "Authorize" and paste token
6. Test all endpoints!
```

---

**Swagger Status:** ✅ Configured and Working
**Default URL:** http://localhost:5000/swagger

