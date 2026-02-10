@echo off
echo ========================================
echo FINAL FIX for Internships 500 Error
echo ========================================
echo.
echo This will:
echo 1. Stop any running processes
echo 2. Clean everything
echo 3. Rebuild project
echo 4. Create/Apply Internships migration
echo 5. Verify database
echo.
echo Press any key to continue...
pause
cls

echo Step 1: Update EF Tools
dotnet tool update --global dotnet-ef
echo.
echo ========================================
echo.

echo Step 2: Navigate to project
cd backend\GradLink.Api
echo Current directory: %CD%
echo.
echo ========================================
echo.

echo Step 3: Clean everything
cd ..
echo Cleaning solution...
dotnet clean
echo.
echo ========================================
echo.

echo Step 4: Restore packages
echo Restoring NuGet packages...
dotnet restore
echo.
echo ========================================
echo.

echo Step 5: Build project
echo Building project...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo BUILD FAILED!
    echo Please fix build errors and try again.
    pause
    exit /b 1
)
echo.
echo Build successful!
echo ========================================
echo.

cd GradLink.Api

echo Step 6: List current migrations
echo.
echo Current migrations in project:
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.
echo ========================================
echo.

echo Step 7: Apply ALL existing migrations first
echo.
echo Applying all migrations to database...
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.
echo ========================================
echo.

echo Step 8: Check if Internships migration exists
echo.
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project . | findstr /i "internship"
if %ERRORLEVEL% EQU 0 (
    echo Internships migration already exists!
    echo Skipping migration creation...
) else (
    echo No Internships migration found.
    echo Creating new migration...
    dotnet ef migrations add AddInternshipsTableFinal --project ../GradLink.Infrastructure --startup-project . --verbose
    echo.
    echo Applying new migration...
    dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
)
echo.
echo ========================================
echo.

echo Step 9: Final verification
echo.
echo All migrations after update:
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.
echo ========================================
echo.
echo IMPORTANT: Check the list above
echo ALL migrations should have "Done" or checkmark
echo.
pause
cls

echo ========================================
echo Starting Backend Server
echo ========================================
echo.
echo Server will start on: http://localhost:5000
echo.
echo After server starts:
echo 1. Open http://localhost:5000/swagger
echo 2. Test GET /api/Internships/my
echo 3. Should return 200 OK (not 500!)
echo.
echo Press Ctrl+C to stop server
echo.
echo ========================================

dotnet run













