@echo off
echo ========================================
echo GradLink - Quick Database Setup
echo ========================================
echo.

echo This script will:
echo 1. Install/Update EF Tools
echo 2. Create Database
echo 3. Apply All Migrations
echo 4. Start Backend Server
echo.
echo Press any key to continue...
pause
cls

echo ========================================
echo Step 1: Installing EF Core Tools
echo ========================================
echo.
dotnet tool update --global dotnet-ef
echo.

echo ========================================
echo Step 2: Checking EF Tools Version
echo ========================================
echo.
dotnet ef --version
echo.
pause

echo ========================================
echo Step 3: Going to Project Directory
echo ========================================
echo.
cd backend\GradLink.Api
echo Current directory: %CD%
echo.

echo ========================================
echo Step 4: Cleaning and Building Project
echo ========================================
echo.
cd ..
dotnet clean
dotnet build
cd GradLink.Api
echo.

echo ========================================
echo Step 5: Creating/Updating Database
echo ========================================
echo.
echo This will create GradLinkDb database...
dotnet ef database update --verbose
echo.

echo ========================================
echo Step 6: Adding Internships Migration
echo ========================================
echo.
dotnet ef migrations add AddInternshipsTable
echo.

echo ========================================
echo Step 7: Applying Internships Migration
echo ========================================
echo.
dotnet ef database update --verbose
echo.

echo ========================================
echo Step 8: Listing All Applied Migrations
echo ========================================
echo.
dotnet ef migrations list
echo.

echo ========================================
echo Database Setup Complete!
echo ========================================
echo.
echo Database: GradLinkDb
echo Tables Created:
echo   - AspNetUsers (Users)
echo   - JobPostings (Jobs)
echo   - Internships (Internships)
echo   - Projects (Projects)
echo   - And more...
echo.
echo Press any key to start Backend server...
pause

cls
echo ========================================
echo Starting Backend Server
echo ========================================
echo.
echo Backend will start on: http://localhost:5000
echo Swagger UI: http://localhost:5000/swagger
echo.
echo Press Ctrl+C to stop the server
echo.

dotnet run













