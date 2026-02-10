@echo off
echo ========================================
echo Complete Fix for Internships Issue
echo ========================================
echo.

echo Step 1: Checking dotnet-ef installation...
dotnet tool list --global | findstr dotnet-ef
if %ERRORLEVEL% NEQ 0 (
    echo Installing dotnet-ef...
    dotnet tool install --global dotnet-ef
) else (
    echo dotnet-ef is already installed, updating...
    dotnet tool update --global dotnet-ef
)
echo.

echo Step 2: Going to API project directory...
cd backend\GradLink.Api
echo Current directory: %CD%
echo.

echo Step 3: Cleaning and building project...
cd ..
dotnet clean
dotnet build
cd GradLink.Api
echo.

echo Step 4: Listing existing migrations...
dotnet ef migrations list
echo.

echo Step 5: Creating Internships migration...
dotnet ef migrations add AddInternshipsTable --verbose
echo.

echo Step 6: Updating database...
dotnet ef database update --verbose
echo.

echo ========================================
echo Migration Complete!
echo ========================================
echo.

echo Checking if Internships table exists in database...
echo You can verify by opening SQL Server Management Studio
echo.

echo Press any key to start the backend server...
pause

echo Starting backend...
dotnet run

pause













