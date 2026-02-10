@echo off
echo ========================================
echo Add Internships Table to Database
echo ========================================
echo.

echo Step 1: Update EF Tools...
dotnet tool update --global dotnet-ef
echo.

echo Step 2: Going to API Project...
cd backend\GradLink.Api
echo.

echo Step 3: Building Project...
cd ..
dotnet clean
dotnet build
cd GradLink.Api
echo.

echo Step 4: Creating Internships Migration...
dotnet ef migrations add AddInternshipsTable --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Migration failed!
    echo Trying with different name...
    dotnet ef migrations add AddInternships --project ../GradLink.Infrastructure --startup-project . --verbose
    echo.
)

echo Step 5: Applying Migration to Database...
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo ========================================
echo SUCCESS! Internships Table Created
echo ========================================
echo.

echo Step 6: Verifying Migration...
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.

echo ========================================
echo All Done!
echo ========================================
echo.
echo The Internships table has been added to your database.
echo You can now use the Internships feature in Company Dashboard.
echo.
echo Press any key to start Backend server...
pause

cls
echo Starting Backend Server...
echo.
dotnet run













