@echo off
echo ========================================
echo Fix Internships 500 Error
echo ========================================
echo.
echo This will properly create the Internships table in database.
echo.
pause

echo Step 1: Updating EF Tools...
dotnet tool update --global dotnet-ef
echo.

echo Step 2: Going to API project...
cd backend\GradLink.Api
echo.

echo Step 3: Cleaning project...
cd ..
dotnet clean
echo.

echo Step 4: Building project...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Build failed! Fix errors and try again.
    pause
    exit /b 1
)
echo.

cd GradLink.Api

echo Step 5: Listing current migrations...
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.

echo Step 6: Removing last migration if exists (in case it was incomplete)...
dotnet ef migrations remove --force --project ../GradLink.Infrastructure --startup-project .
echo.

echo Step 7: Creating fresh Internships migration...
dotnet ef migrations add AddInternshipsTableFinal --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

if %ERRORLEVEL% NEQ 0 (
    echo Migration creation failed!
    pause
    exit /b 1
)

echo Step 8: Applying migration to database...
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

if %ERRORLEVEL% NEQ 0 (
    echo Database update failed!
    echo.
    echo Possible issues:
    echo 1. Database connection failed
    echo 2. Table already exists
    echo.
    echo Let me try to just update without creating new migration...
    echo.
    dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
)

echo.
echo Step 9: Verifying migrations...
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.

echo ========================================
echo Check the migrations list above.
echo All migrations should have a checkmark.
echo ========================================
echo.
pause

echo Starting Backend Server...
echo.
echo Backend will start on: http://localhost:5000
echo After it starts, check Swagger and try the Internships endpoints.
echo.
echo Press Ctrl+C to stop when done testing.
echo.

dotnet run













