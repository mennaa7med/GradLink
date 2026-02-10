@echo off
echo ========================================
echo Remove Internships Table from Database
echo ========================================
echo.
echo WARNING: This will delete the Internships table
echo and all its data from the database!
echo.
echo Press Ctrl+C to cancel, or any other key to continue...
pause
cls

echo Step 1: Update EF Tools
dotnet tool update --global dotnet-ef
echo.

echo Step 2: Navigate to project
cd backend\GradLink.Api
echo.

echo Step 3: Clean and Build
cd ..
dotnet clean
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Build failed! Please fix errors first.
    pause
    exit /b 1
)
echo.

cd GradLink.Api

echo Step 4: Create Migration to Remove Internships
dotnet ef migrations add RemoveInternshipsTable --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo Step 5: Apply Migration (Drop Internships table)
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo ========================================
echo Internships table removed from database!
echo ========================================
echo.

echo Step 6: Verify migrations
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.

pause

echo Starting Backend Server...
dotnet run













