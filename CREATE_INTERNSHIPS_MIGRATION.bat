@echo off
echo ========================================
echo Creating Internships Database Migration
echo ========================================
echo.

cd backend\GradLink.Api

echo Creating migration...
dotnet ef migrations add AddInternshipsTable

echo.
echo ========================================
echo Updating Database...
echo ========================================
echo.

dotnet ef database update

echo.
echo ========================================
echo Migration Complete!
echo ========================================
echo.
pause














