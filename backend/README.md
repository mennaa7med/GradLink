# GradLink Backend API

A comprehensive .NET 8 backend for the GradLink platform - a graduate networking and career platform.

## Features

- **Clean Architecture** - Separation of concerns with Domain, Application, Infrastructure, and API layers
- **Authentication & Authorization** - JWT-based authentication with refresh tokens, role-based access control
- **User Management** - ASP.NET Core Identity with custom user profiles
- **Project Management** - Create, manage, and track projects with tasks
- **Resume Management** - Upload, analyze, and manage resumes with background processing
- **Job Postings** - CRUD operations for job postings
- **Job-Resume Matching** - Intelligent matching algorithm between resumes and job postings
- **Real-time Chat** - SignalR-based chat functionality for user communication
- **AI Integration** - Proxy for Gemini AI API
- **File Storage** - Support for both local and Azure Blob Storage
- **Background Processing** - Queue-based resume analysis
- **Rate Limiting** - IP-based rate limiting for API protection
- **Logging** - Structured logging with Serilog
- **API Documentation** - Swagger/OpenAPI documentation

## Technology Stack

- **.NET 8** - Latest LTS version
- **Entity Framework Core 8** - ORM with SQLite/SQL Server support
- **ASP.NET Core Identity** - User authentication and authorization
- **JWT Bearer Authentication** - Secure token-based authentication
- **SignalR** - Real-time bidirectional communication
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **AspNetCoreRateLimit** - Rate limiting middleware
- **Azure Storage Blobs** - Cloud file storage (optional)

## Architecture

### Clean Architecture Layers

```
├── GradLink.Domain          # Core business entities
├── GradLink.Application     # Business logic, interfaces, DTOs
├── GradLink.Infrastructure  # Data access, external services
└── GradLink.Api             # Web API, controllers, hubs
```

### Domain Entities

- **ApplicationUser** - Extended ASP.NET Identity user
- **Project** - User projects
- **TaskItem** - Project tasks
- **Resume** - Resume files and analysis results
- **JobPosting** - Job postings
- **Match** - Resume-job matches
- **Conversation** - Chat conversations
- **ChatMessage** - Chat messages
- **RefreshToken** - JWT refresh tokens

## Getting Started - Step by Step

Follow these steps to get the GradLink backend up and running on your machine.

### Prerequisites

Before you begin, ensure you have the following installed:

1. **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** - Required
   - Download and install .NET 8 SDK
   - Verify installation: `dotnet --version` (should show 8.0.x)

2. **SQL Server** - Required (One of the following):
   - **Option A:** [SQL Server 2022 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Free, Windows)
   - **Option B:** [SQL Server Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Free, Full-featured)
   - **Option C:** SQL Server via Docker (Cross-platform):
     ```bash
     docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
     ```

3. **Code Editor** - Recommended (One of the following):
   - [Visual Studio 2022](https://visualstudio.microsoft.com/) (Windows/Mac)
   - [Visual Studio Code](https://code.visualstudio.com/) with C# extension
   - [JetBrains Rider](https://www.jetbrains.com/rider/)

### Step-by-Step Installation

#### Step 1: Navigate to Backend Directory

```bash
cd backend
```

#### Step 2: Verify .NET Installation

```bash
dotnet --version
```

Expected output: `8.0.x` (or higher)

#### Step 3: Restore NuGet Packages

This downloads all required dependencies:

```bash
dotnet restore
```

Wait for all packages to download. You should see "Restore succeeded."

#### Step 4: Configure SQL Server Connection

Open `GradLink.Api/appsettings.json` and update the connection string if needed:

**Current default configuration:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Common configurations:**

- **Local SQL Server with SA account** (default):
  ```
  Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True
  ```

- **Windows Authentication**:
  ```
  Server=localhost;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True
  ```

- **Named Instance**:
  ```
  Server=localhost\SQLEXPRESS;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True
  ```

#### Step 5: Verify SQL Server is Running

**Windows:**
```powershell
# Check if SQL Server service is running
Get-Service MSSQLSERVER
```

**Docker:**
```bash
# Check if container is running
docker ps | grep sqlserver

# Start if stopped
docker start sqlserver
```

**Test Connection:**
```bash
# Using sqlcmd (if installed)
sqlcmd -S localhost -U sa -P YourStrong@Password123 -Q "SELECT @@VERSION"
```

#### Step 6: Navigate to API Project

```bash
cd GradLink.Api
```

#### Step 7: Build the Project

Compile the application to check for errors:

```bash
dotnet build
```

Expected output: `Build succeeded. 0 Warning(s) 0 Error(s)`

#### Step 8: Initialize the Database

The application will automatically create and configure the database on first run, but you can do it manually:

**Option A: Automatic (Recommended)**
Just run the application (Step 9), and it will handle database creation automatically.

**Option B: Manual**
```bash
# Create migration (if not exists)
dotnet ef database update

# This will:
# - Create the database if it doesn't exist
# - Create all tables
# - Run all migrations
```

#### Step 9: Run the Application

Start the API server:

```bash
dotnet run
```

**Expected output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**First run will:**
- ✅ Create database `GradLinkDb` (if doesn't exist)
- ✅ Apply all migrations
- ✅ Create tables
- ✅ Seed roles (Admin, Student, Company, Mentor, Sponsor)
- ✅ Create default admin user

#### Step 10: Verify Installation

Open your browser and navigate to:

```
https://localhost:5001/swagger
```

You should see the **Swagger UI** with all API endpoints.

### Step 11: Test the API

#### A. Test with Default Admin Account

1. In Swagger UI, find **POST /api/auth/login**
2. Click "Try it out"
3. Use these credentials:
   ```json
   {
     "email": "admin@gradlink.com",
     "password": "Admin@123"
   }
   ```
4. Click "Execute"
5. You should receive an `accessToken` and `refreshToken`

#### B. Authorize Swagger

1. Copy the `accessToken` from the response
2. Click the **"Authorize"** button at the top of Swagger UI
3. Enter: `Bearer YOUR_ACCESS_TOKEN_HERE`
4. Click "Authorize"
5. Now you can test all protected endpoints!

#### C. Create a New User (Optional)

1. Find **POST /api/auth/register**
2. Click "Try it out"
3. Enter user details:
   ```json
   {
     "email": "user@example.com",
     "password": "UserPass123",
     "fullName": "John Doe",
     "phoneNumber": "+1234567890"
   }
   ```
4. Click "Execute"

### Alternative: Run with Hot Reload

For development with automatic code reloading:

```bash
cd GradLink.Api
dotnet watch run
```

Changes to code will automatically reload the application.

### Verify Database Creation

**Using SQL Server Management Studio (SSMS):**
1. Connect to your SQL Server instance
2. Look for database named `GradLinkDb`
3. Expand Tables - you should see:
   - AspNetUsers
   - AspNetRoles
   - Projects
   - Tasks
   - Resumes
   - JobPostings
   - Matches
   - Conversations
   - ChatMessages
   - RefreshTokens

**Using SQL Query:**
```sql
USE GradLinkDb;
GO

-- List all tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Verify admin user
SELECT * FROM AspNetUsers WHERE Email = 'admin@gradlink.com';

-- Verify roles
SELECT * FROM AspNetRoles;
```

### Troubleshooting

#### Issue: "Cannot connect to SQL Server"

**Solution:**
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Test connection with `sqlcmd` or SSMS

#### Issue: "Build failed"

**Solution:**
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

#### Issue: "Port already in use"

**Solution:**
Edit `Properties/launchSettings.json` and change ports:
```json
"applicationUrl": "https://localhost:5002;http://localhost:5003"
```

#### Issue: "Database creation failed"

**Solution:**
- Ensure SQL Server user has CREATE DATABASE permission
- Or create database manually:
  ```sql
  CREATE DATABASE GradLinkDb;
  GO
  ```

#### Issue: "Migration failed"

**Solution:**
```bash
# Remove last migration
dotnet ef migrations remove --project ../GradLink.Infrastructure

# Re-create migration
dotnet ef migrations add Initial --project ../GradLink.Infrastructure

# Apply migration
dotnet ef database update
```

### Next Steps After Installation

1. **✅ Change Default Admin Password**
   - Login as admin
   - Use the API to update password

2. **✅ Configure CORS**
   - Add your frontend URL to `appsettings.json`:
     ```json
     "Cors": {
       "AllowedOrigins": ["http://localhost:5176", "http://localhost:3000"]
     }
     ```

3. **✅ Set JWT Secret** (Production)
   - Generate a secure random string (32+ characters)
   - Update in `appsettings.json`:
     ```json
     "Jwt": {
       "Secret": "YOUR_SECURE_RANDOM_STRING_HERE"
     }
     ```

4. **✅ Configure Gemini AI** (Optional)
   - Get API key from [Google AI Studio](https://makersuite.google.com/app/apikey)
   - Add to `appsettings.json`:
     ```json
     "Gemini": {
       "ApiKey": "your-api-key-here"
     }
     ```

5. **✅ Set Up File Storage** (Optional)
   - For Azure Blob Storage, update `appsettings.json`:
     ```json
     "FileStorage": {
       "Mode": "Azure",
       "AzureBlobConnectionString": "your-connection-string",
       "AzureBlobContainerName": "gradlink-files"
     }
     ```

### Running in Different Environments

**Development:**
```bash
dotnet run --environment Development
```

**Production:**
```bash
dotnet run --environment Production
```

**Staging:**
```bash
dotnet run --environment Staging
```

### Quick Reference Commands

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Run with hot reload
dotnet watch run

# Create migration
dotnet ef migrations add MigrationName --project ../GradLink.Infrastructure

# Update database
dotnet ef database update

# Run tests
dotnet test

# Publish for deployment
dotnet publish -c Release -o ./publish
```

### Default Admin Account

After the first run, a default admin account will be created:

- **Email:** admin@gradlink.com
- **Password:** Admin@123

**⚠️ Important:** Change this password immediately in production!

## Configuration

### Database

#### SQLite (Default)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=gradlink.db"
  },
  "DatabaseProvider": "Sqlite"
}
```

#### SQL Server

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLink;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "DatabaseProvider": "SqlServer"
}
```

### JWT Configuration

```json
{
  "Jwt": {
    "Secret": "YourSuperSecretKeyAtLeast32Characters",
    "Issuer": "GradLinkApi",
    "Audience": "GradLinkClient",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### File Storage

#### Local Storage (Default)

```json
{
  "FileStorage": {
    "Mode": "Local",
    "LocalPath": "./uploads"
  }
}
```

#### Azure Blob Storage

```json
{
  "FileStorage": {
    "Mode": "Azure",
    "AzureBlobConnectionString": "your-connection-string",
    "AzureBlobContainerName": "gradlink-files"
  }
}
```

### Resume Analyzer Integration

Configure the Python analyzer service URL:

```json
{
  "Analyzer": {
    "PythonAnalyzerUrl": "http://localhost:8000",
    "TimeoutSeconds": 30
  }
}
```

If not configured, a fallback analyzer will be used.

### CORS

```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:5176",
      "http://localhost:3000",
      "https://yourdomain.com"
    ]
  }
}
```

### Rate Limiting

```json
{
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 100
      },
      {
        "Endpoint": "*",
        "Period": "1h",
        "Limit": 1000
      }
    ]
  }
}
```

## API Endpoints

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and get JWT tokens
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - Logout and revoke tokens

### Users

- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/me` - Get current user profile
- `PUT /api/users/me` - Update current user profile
- `POST /api/users/me/avatar` - Upload profile picture

### Projects

- `GET /api/projects` - Get user's projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create a new project
- `PUT /api/projects/{id}` - Update a project
- `DELETE /api/projects/{id}` - Delete a project

### Project Tasks

- `GET /api/projects/{projectId}/tasks` - Get project tasks
- `POST /api/projects/{projectId}/tasks` - Create a new task
- `PUT /api/projects/{projectId}/tasks/{taskId}` - Update a task
- `DELETE /api/projects/{projectId}/tasks/{taskId}` - Delete a task

### Resumes

- `GET /api/resumes` - Get user's resumes
- `GET /api/resumes/{id}` - Get resume by ID
- `POST /api/resumes/upload` - Upload a resume (triggers background analysis)
- `DELETE /api/resumes/{id}` - Delete a resume

### Jobs

- `GET /api/jobs` - Get active job postings
- `GET /api/jobs/my` - Get user's job postings
- `GET /api/jobs/{id}` - Get job by ID
- `POST /api/jobs` - Create a new job posting
- `PUT /api/jobs/{id}` - Update a job posting
- `DELETE /api/jobs/{id}` - Delete a job posting

### Matches

- `GET /api/matches/resume/{resumeId}` - Get matches for a resume
- `GET /api/matches/job/{jobId}` - Get matches for a job
- `POST /api/matches/run/{resumeId}` - Run matching algorithm

### Conversations

- `GET /api/conversations` - Get user's conversations
- `GET /api/conversations/{id}` - Get conversation with messages
- `POST /api/conversations` - Create or get conversation with user

### Chat (SignalR)

- **Hub URL:** `/hubs/chat`
- **Methods:**
  - `SendMessage(recipientId, message)` - Send a message
  - `MarkAsRead(conversationId)` - Mark messages as read
- **Events:**
  - `ReceiveMessage` - Receive incoming message
  - `MessageSent` - Confirmation of sent message

### Admin

- `GET /api/admin/stats` - Get platform statistics (Admin only)
- `GET /api/admin/users` - Get all users with pagination (Admin only)

### Chat Proxy

- `POST /api/chatproxy/gemini` - Proxy requests to Gemini AI

## API Documentation

When running in development mode, Swagger UI is available at:

```
https://localhost:5001/swagger
```

## User Roles

The system supports the following roles:

- **Admin** - Full system access
- **Student** - Student users
- **Company** - Company representatives
- **Mentor** - Mentors
- **Sponsor** - Sponsors

Roles are assigned during registration (default: Student) and can be managed by admins.

## Background Services

### Resume Analysis Queue

Resumes are analyzed in the background using a queue-based approach:

1. User uploads resume
2. Resume is saved to storage
3. Analysis job is queued
4. Background service processes the queue
5. Results are saved to database

## SignalR Real-time Chat

### Client Connection

```javascript
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5001/hubs/chat", {
    accessTokenFactory: () => yourAccessToken
  })
  .build();

await connection.start();
```

### Send Message

```javascript
await connection.invoke("SendMessage", recipientId, message);
```

### Receive Messages

```javascript
connection.on("ReceiveMessage", (message) => {
  console.log("Received message:", message);
});
```

## Database Migrations

### Create a new migration

```bash
cd GradLink.Api
dotnet ef migrations add MigrationName --project ../GradLink.Infrastructure
```

### Apply migrations

```bash
dotnet ef database update
```

### Remove last migration

```bash
dotnet ef migrations remove --project ../GradLink.Infrastructure
```

## Production Deployment

### Important Security Steps

1. **Change JWT Secret** - Use a cryptographically secure random string
2. **Update Admin Password** - Change the default admin password
3. **Configure HTTPS** - Use proper SSL certificates
4. **Set Production ConnectionString** - Use a production database
5. **Configure CORS** - Restrict to your frontend domain
6. **Enable Rate Limiting** - Protect against abuse
7. **Set up Logging** - Configure appropriate log levels and destinations
8. **Secure File Storage** - Use Azure Blob Storage with proper access controls
9. **Environment Variables** - Use environment variables for sensitive configuration

### Environment Variables

You can override configuration using environment variables:

```bash
export ConnectionStrings__DefaultConnection="Server=..."
export Jwt__Secret="your-secret-key"
export Gemini__ApiKey="your-api-key"
```

### Docker Support (Coming Soon)

A Dockerfile will be added for containerized deployment.

## Logging

Logs are written to:

- **Console** - All environments
- **Files** - `logs/gradlink-YYYYMMDD.txt` (rolling daily)

Log levels can be configured in `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## Testing

### Run all tests

```bash
dotnet test
```

(Note: Test projects need to be added)

## Troubleshooting

### Database Connection Issues

- Verify connection string
- Ensure database server is running
- Check firewall settings
- Run migrations: `dotnet ef database update`

### Authentication Issues

- Verify JWT configuration
- Check token expiration
- Ensure CORS is properly configured

### File Upload Issues

- Check file storage configuration
- Verify directory permissions (Local storage)
- Check Azure connection string (Azure Blob Storage)

### SignalR Connection Issues

- Ensure CORS allows SignalR connections
- Check WebSocket support
- Verify JWT token is passed correctly

## Contributing

1. Follow Clean Architecture principles
2. Write unit tests for new features
3. Follow C# coding conventions
4. Update documentation
5. Create pull requests with clear descriptions

## License

This project is licensed under the MIT License.

## Support

For issues and questions:
- Create an issue in the repository
- Contact the development team

## Version History

### v1.0.0 (Current)
- Initial release
- Complete authentication system
- Project and task management
- Resume upload and analysis
- Job postings and matching
- Real-time chat
- Admin panel
- AI integration (Gemini)

---

Built with ❤️ using .NET 8

