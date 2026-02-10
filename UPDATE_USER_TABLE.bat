@echo off
echo ============================================
echo   GradLink - Update User Table Migration
echo ============================================
echo.

cd backend\GradLink.Api

echo Step 1: Creating migration for updated User table...
dotnet ef migrations add UpdateUserTableComplete --project ..\GradLink.Infrastructure --startup-project . --context AppDbContext

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Failed to create migration!
    echo Please check if there are any compilation errors.
    pause
    exit /b 1
)

echo.
echo Step 2: Applying migration to database...
dotnet ef database update --project ..\GradLink.Infrastructure --startup-project . --context AppDbContext

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Failed to apply migration!
    echo Please check the database connection.
    pause
    exit /b 1
)

echo.
echo ============================================
echo   Migration completed successfully!
echo ============================================
echo.
echo New tables created:
echo   - StudentProfiles
echo   - CompanyProfiles
echo   - MentorProfiles
echo.
echo ApplicationUser table updated with new fields.
echo.
pause






