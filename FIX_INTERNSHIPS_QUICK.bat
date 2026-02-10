@echo off
echo ========================================
echo Quick Fix for Internships Issue
echo ========================================
echo.

echo Step 1: Creating Migration...
cd backend\GradLink.Api
dotnet ef migrations add AddInternshipsTable --force
echo.

echo Step 2: Updating Database...
dotnet ef database update
echo.

echo Step 3: Building Backend...
cd ..
dotnet clean
dotnet build
echo.

echo Step 4: Starting Backend...
cd GradLink.Api
echo.
echo Backend is starting...
echo After backend starts, open another terminal and run: npm run dev
echo.
dotnet run














