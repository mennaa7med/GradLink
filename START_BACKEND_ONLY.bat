@echo off
echo ====================================
echo Starting Backend API ONLY
echo ====================================
echo.

cd /d "%~dp0backend\GradLink.Api"

echo Current directory: %cd%
echo.

echo Checking .NET SDK...
dotnet --version
echo.

echo Starting Backend API on Port 5000...
echo.

dotnet run --urls http://localhost:5000

echo.
echo ====================================
echo Backend stopped or failed to start
echo ====================================
pause

