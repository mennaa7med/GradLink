@echo off
echo ========================================
echo Create Initial Migration for GradLink
echo ========================================
echo.

echo Checking EF Tools...
dotnet tool update --global dotnet-ef
echo.

echo Going to API project...
cd backend\GradLink.Api
echo Current directory: %CD%
echo.

echo Cleaning project...
cd ..
dotnet clean
echo.

echo Building project...
dotnet build
echo.

cd GradLink.Api
echo Creating Initial Migration...
dotnet ef migrations add InitialCreate --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo Applying Migration to Database...
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo ========================================
echo Adding Internships Migration...
echo ========================================
dotnet ef migrations add AddInternshipsTable --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo Applying Internships Migration...
dotnet ef database update --project ../GradLink.Infrastructure --startup-project . --verbose
echo.

echo ========================================
echo Listing All Migrations...
echo ========================================
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
echo.

echo ========================================
echo Migration Complete!
echo ========================================
echo.
echo Press any key to start backend server...
pause

dotnet run













