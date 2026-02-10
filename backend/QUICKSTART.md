# GradLink Backend - Quick Start Guide

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed

## Quick Start (5 minutes)

### 1. Navigate to the backend directory

```bash
cd backend
```

### 2. Restore packages

```bash
dotnet restore
```

### 3. Navigate to API project

```bash
cd GradLink.Api
```

### 4. Run the application

```bash
dotnet run
```

The API will start on:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

### 5. Access Swagger UI

Open your browser and navigate to:

```
https://localhost:5001/swagger
```

Here you can see all available API endpoints and test them interactively.

### 6. Default Admin Credentials

The system automatically creates an admin account on first run:

- **Email:** admin@gradlink.com
- **Password:** Admin@123

⚠️ **Important:** Change this password immediately!

## Testing the API

### 1. Register a New User

**POST** `/api/auth/register`

```json
{
  "email": "user@example.com",
  "password": "YourPassword123",
  "fullName": "John Doe",
  "phoneNumber": "+1234567890"
}
```

### 2. Login

**POST** `/api/auth/login`

```json
{
  "email": "user@example.com",
  "password": "YourPassword123"
}
```

Response will include:
- `accessToken` - Use this in the Authorization header
- `refreshToken` - Use this to get new access tokens

### 3. Use the Access Token

For subsequent requests, add the Authorization header:

```
Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
```

In Swagger UI, click the "Authorize" button and enter: `Bearer YOUR_ACCESS_TOKEN`

## Project Structure

```
backend/
├── GradLink.Domain/          # Core entities and domain logic
├── GradLink.Application/     # Business logic, interfaces, DTOs
├── GradLink.Infrastructure/  # Data access, external services
└── GradLink.Api/             # REST API and SignalR hubs
```

## Key Features Available

✅ **Authentication & Authorization** - JWT-based with refresh tokens
✅ **User Management** - Profile management with avatars
✅ **Project Management** - Create and manage projects with tasks
✅ **Resume Management** - Upload and analyze resumes (with background processing)
✅ **Job Postings** - CRUD operations for job postings
✅ **Job-Resume Matching** - Intelligent matching algorithm
✅ **Real-time Chat** - SignalR-based chat system
✅ **AI Integration** - Gemini AI proxy
✅ **Admin Panel** - Statistics and user management

## Configuration

Edit `appsettings.json` to configure:

### Database

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=gradlink.db"
},
"DatabaseProvider": "Sqlite"
```

### JWT Secret

⚠️ **Change in production!**

```json
"Jwt": {
  "Secret": "YourSuperSecretKeyAtLeast32Characters"
}
```

### CORS

Add your frontend URLs:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:5176",
    "http://localhost:3000"
  ]
}
```

### Gemini AI (Optional)

```json
"Gemini": {
  "ApiKey": "your-gemini-api-key"
}
```

### Resume Analyzer (Optional)

```json
"Analyzer": {
  "PythonAnalyzerUrl": "http://localhost:8000"
}
```

## Database

The application uses SQLite by default. The database file (`gradlink.db`) is created automatically on first run in the API project directory.

### View Database

You can use tools like:
- [DB Browser for SQLite](https://sqlitebrowser.org/)
- [SQLiteStudio](https://sqlitestudio.pl/)

### Migrations

If you make changes to entities, create a migration:

```bash
dotnet ef migrations add YourMigrationName --project ../GradLink.Infrastructure
dotnet ef database update
```

## Common Issues

### Port Already in Use

If ports 5000 or 5001 are already in use, edit `Properties/launchSettings.json`:

```json
"applicationUrl": "https://localhost:5002;http://localhost:5003"
```

### CORS Errors

Make sure your frontend URL is in the `Cors:AllowedOrigins` array in `appsettings.json`

### Database Errors

Delete the database file and restart:

```bash
rm gradlink.db*
dotnet run
```

## Next Steps

1. **Change Admin Password** - Login as admin and change the password
2. **Configure CORS** - Add your frontend URLs
3. **Set up Gemini AI** - Get an API key from Google AI Studio
4. **Configure Resume Analyzer** - Set up the Python analyzer service (optional)
5. **Customize** - Modify entities, add features, customize to your needs

## Development

### Hot Reload

.NET 8 supports hot reload. Changes to code will automatically reload:

```bash
dotnet watch run
```

### Build

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

(Note: Test projects need to be added)

## Production Deployment

See the main [README.md](README.md) for detailed production deployment instructions.

## API Documentation

Full API documentation is available at `/swagger` when running the application.

Key endpoints:

- `/api/auth/*` - Authentication
- `/api/users/*` - User management
- `/api/projects/*` - Project management
- `/api/resumes/*` - Resume management
- `/api/jobs/*` - Job postings
- `/api/matches/*` - Job-resume matching
- `/api/conversations/*` - Chat
- `/api/admin/*` - Admin endpoints
- `/hubs/chat` - SignalR chat hub

## Support

For more information, see the [README.md](README.md) or create an issue.

---

Happy coding! 🚀

