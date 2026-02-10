# Entity Framework Core Migration Commands

## Visual Studio - Package Manager Console

### Step 1: Open Package Manager Console
- Go to: `Tools` → `NuGet Package Manager` → `Package Manager Console`

### Step 2: Select Correct Projects
In the Package Manager Console window:
- **Default project:** `GradLink.Infrastructure` (where DbContext is)
- **Startup project:** `GradLink.Api` (set in Solution Explorer)

### Step 3: Create Initial Migration
```powershell
Add-Migration InitialCreate
```

### Step 4: Apply Migration to Database
```powershell
Update-Database
```

### Step 5: Add Internships Migration
```powershell
Add-Migration AddInternshipsTable
Update-Database
```

---

## PowerShell / CMD / Terminal

### Step 1: Install EF Tools (if not installed)
```powershell
dotnet tool install --global dotnet-ef
# or update if already installed
dotnet tool update --global dotnet-ef
```

### Step 2: Navigate to API Project
```powershell
cd "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"
```

### Step 3: Create Initial Migration
```powershell
dotnet ef migrations add InitialCreate --project ../GradLink.Infrastructure --startup-project .
```

### Step 4: Apply Migration to Database
```powershell
dotnet ef database update --project ../GradLink.Infrastructure --startup-project .
```

### Step 5: Add Internships Migration
```powershell
dotnet ef migrations add AddInternshipsTable --project ../GradLink.Infrastructure --startup-project .
dotnet ef database update --project ../GradLink.Infrastructure --startup-project .
```

---

## Verify Migrations

### List all migrations:
```powershell
# Package Manager Console
Get-Migration

# PowerShell/Terminal
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
```

### Check database:
```powershell
# Package Manager Console
Update-Database -Verbose

# PowerShell/Terminal
dotnet ef database update --verbose --project ../GradLink.Infrastructure --startup-project .
```

---

## Common Issues

### Issue 1: "Build failed"
**Solution:**
```powershell
# Clean and rebuild
dotnet clean
dotnet build
```

### Issue 2: "No DbContext was found"
**Solution:**
Make sure you're in the correct directory (GradLink.Api) and the project references are correct.

### Issue 3: "Unable to create migration"
**Solution:**
```powershell
# Remove last migration
Remove-Migration
# or
dotnet ef migrations remove --project ../GradLink.Infrastructure --startup-project .

# Try again
Add-Migration InitialCreate
```

---

## Project Structure Reference

```
backend/
├── GradLink.Api/              ← Startup project
├── GradLink.Application/
├── GradLink.Domain/
└── GradLink.Infrastructure/   ← DbContext location (Default project)
    ├── Persistence/
    │   └── AppDbContext.cs
    └── Migrations/            ← Migrations will be created here
```

---

## Current Connection String

From appsettings.json:
```
Server=db32798.public.databaseasp.net
Database=db32798
User Id=db32798
Password=Ao7_6T+m?sY2
```

This is a remote SQL Server database, so make sure you have internet connection.













