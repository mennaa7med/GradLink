@echo off
echo ========================================
echo Setup Internships Feature - Final
echo ========================================
echo.
echo This will setup Internships to work like Jobs and Projects
echo.
pause
cls

echo Step 1: Update EF Tools
dotnet tool update --global dotnet-ef
echo.

echo Step 2: Navigate to API project
cd backend\GradLink.Api
echo.

echo Step 3: Clean and Build
cd ..
dotnet clean
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Build failed! Please fix errors.
    pause
    exit /b 1
)
echo.

cd GradLink.Api

echo Step 4: Create Internships Migration
dotnet ef migrations add AddInternshipsFeature --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo Step 5: Apply Migration to Database
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo Step 6: Verify Migrations
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.

echo ========================================
echo SUCCESS! Internships Feature Ready
echo ========================================
echo.
echo The following are now available:
echo - Internships section in Company Dashboard sidebar
echo - Add/Edit/Delete Internships functionality
echo - Internships display in Career page
echo - Backend APIs at /api/internships
echo.
pause

echo Starting Backend Server...
echo.
dotnet run













