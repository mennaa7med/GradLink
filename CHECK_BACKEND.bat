@echo off
echo ====================================
echo Checking Backend Setup
echo ====================================
echo.

cd /d "%~dp0backend\GradLink.Api"

echo Current directory: %cd%
echo.

echo 1. Checking .NET SDK...
dotnet --version
if errorlevel 1 (
    echo ERROR: .NET SDK not found!
    echo Please install .NET SDK 8.0 or higher
    pause
    exit /b 1
)
echo OK - .NET SDK found
echo.

echo 2. Checking project file...
if exist "GradLink.Api.csproj" (
    echo OK - Project file found
) else (
    echo ERROR: GradLink.Api.csproj not found!
    pause
    exit /b 1
)
echo.

echo 3. Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)
echo OK - Packages restored
echo.

echo 4. Building project...
dotnet build
if errorlevel 1 (
    echo ERROR: Build failed! Check errors above.
    pause
    exit /b 1
)
echo OK - Build successful
echo.

echo ====================================
echo Backend is ready to run!
echo ====================================
echo.
echo Now you can run: START_BACKEND_ONLY.bat
echo.
pause

