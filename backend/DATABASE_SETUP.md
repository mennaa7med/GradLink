# SQL Server Database Setup Guide

## Current Configuration

The application is now configured to use **Microsoft SQL Server** instead of SQLite.

### Connection Strings

**Production (`appsettings.json`):**
```
Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true
```

**Development (`appsettings.Development.json`):**
```
Server=localhost;Database=GradLinkDb_Dev;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true
```

## Prerequisites

### 1. Install SQL Server

**Windows:**
- Download [SQL Server 2022 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Or use [SQL Server Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (free)

**macOS/Linux:**
- Use Docker:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Install SQL Server Management Studio (Optional)

- Download [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Or use [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio/) (cross-platform)

## Configuration Options

### Option 1: Using Default Configuration (localhost with SA)

If using the default connection string, ensure SQL Server is running on localhost with:
- **Server**: localhost (or 127.0.0.1 or .)
- **User**: sa
- **Password**: YourStrong@Password123

### Option 2: Windows Authentication

Update connection string to:
```json
"DefaultConnection": "Server=localhost;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### Option 3: Custom SQL Server Instance

Update with your server details:
```json
"DefaultConnection": "Server=YOUR_SERVER;Database=GradLinkDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### Option 4: Azure SQL Database

```json
"DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Database=GradLinkDb;User ID=yourusername@yourserver;Password=yourpassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Database Setup Steps

### 1. Ensure SQL Server is Running

**Windows Service:**
```powershell
# Check status
Get-Service MSSQLSERVER

# Start if stopped
Start-Service MSSQLSERVER
```

**Docker:**
```bash
docker start sqlserver
```

### 2. Create Database (Automatic)

The application will automatically:
- Create the database if it doesn't exist
- Run all migrations
- Seed initial data (roles and admin user)

Just run:
```bash
cd GradLink.Api
dotnet run
```

### 3. Create Database (Manual - Optional)

If you prefer to create the database manually:

```sql
CREATE DATABASE GradLinkDb;
GO
```

Then run migrations:
```bash
cd GradLink.Api
dotnet ef database update
```

## Creating Migrations

When you modify entities, create a new migration:

```bash
cd GradLink.Api
dotnet ef migrations add YourMigrationName --project ../GradLink.Infrastructure
```

Apply the migration:
```bash
dotnet ef database update
```

## Troubleshooting

### Error: Cannot connect to SQL Server

**Check if SQL Server is running:**
```powershell
# Windows
Get-Service MSSQLSERVER

# SQL Server Browser (for named instances)
Get-Service SQLBrowser
```

**Test connection:**
```bash
# Using sqlcmd
sqlcmd -S localhost -U sa -P YourStrong@Password123

# Or
sqlcmd -S localhost -E  # Windows Authentication
```

### Error: Login failed for user 'sa'

1. **Check password complexity** - SQL Server requires strong passwords
2. **Enable SQL Server Authentication** (if using mixed mode)
3. **Check if SA account is enabled**

To enable SA and mixed mode authentication:
```sql
-- Enable SA account
ALTER LOGIN sa ENABLE;
GO

-- Set password
ALTER LOGIN sa WITH PASSWORD = 'YourStrong@Password123';
GO
```

### Error: Database creation failed

Ensure the SQL Server user has permission to create databases:

```sql
-- Grant create database permission
USE master;
GO
GRANT CREATE DATABASE TO [your_user];
GO
```

### Connection Timeout

Increase timeout in connection string:
```json
"DefaultConnection": "Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true;Connection Timeout=60"
```

### Port Issues

If SQL Server is not on default port 1433:
```json
"DefaultConnection": "Server=localhost,1434;Database=GradLinkDb;..."
```

## Verifying Database

### Using SSMS

1. Connect to SQL Server
2. Expand Databases
3. You should see `GradLinkDb` (or `GradLinkDb_Dev` in development)
4. Expand Tables - you should see:
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
   - And other Identity tables

### Using SQL Query

```sql
USE GradLinkDb;
GO

-- List all tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Check admin user
SELECT * FROM AspNetUsers WHERE Email = 'admin@gradlink.com';

-- Check roles
SELECT * FROM AspNetRoles;
```

## Connection String Parameters Explained

- **Server**: SQL Server instance (localhost, IP, or server name)
- **Database**: Database name
- **User Id**: SQL Server user
- **Password**: User password
- **TrustServerCertificate=True**: Accept self-signed certificates (dev)
- **MultipleActiveResultSets=true**: Allow multiple result sets
- **Integrated Security=true**: Use Windows Authentication (alternative to User Id/Password)
- **Encrypt=True**: Encrypt connection (recommended for production)
- **Connection Timeout**: Timeout in seconds (default 15)

## Security Best Practices

### Development
✅ Use separate database for development
✅ Use strong passwords
✅ Don't commit passwords to Git (use User Secrets)

### Production
✅ Use Windows Authentication or Azure AD when possible
✅ Use encrypted connections (Encrypt=True)
✅ Store connection strings in environment variables or Azure Key Vault
✅ Use least privilege principle for database user
✅ Enable SQL Server audit logging
✅ Regular backups

## Using User Secrets (Recommended for Development)

Instead of storing connection strings in appsettings.json:

```bash
cd GradLink.Api

# Initialize user secrets
dotnet user-secrets init

# Set connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=GradLinkDb_Dev;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True"
```

Then remove the connection string from appsettings.Development.json

## Backup and Restore

### Backup
```sql
BACKUP DATABASE GradLinkDb
TO DISK = 'C:\Backup\GradLinkDb.bak'
WITH FORMAT;
```

### Restore
```sql
RESTORE DATABASE GradLinkDb
FROM DISK = 'C:\Backup\GradLinkDb.bak'
WITH REPLACE;
```

## Docker Compose Setup

A `docker-compose.yml` is included with SQL Server support (commented out). To use it:

1. Uncomment the SQL Server service in `docker-compose.yml`
2. Update connection string to: `Server=sqlserver;Database=GradLinkDb;...`
3. Run: `docker-compose up -d`

## Need Help?

- [SQL Server Documentation](https://learn.microsoft.com/en-us/sql/sql-server/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Connection Strings Reference](https://www.connectionstrings.com/sql-server/)

---

**Current Status:** ✅ Configured for MS SQL Server
**Database Provider:** SqlServer
**Default Database:** GradLinkDb (Production) / GradLinkDb_Dev (Development)

