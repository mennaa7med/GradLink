@echo off
echo ========================================
echo   Fix Backend DLL Lock Issue
echo ========================================
echo.
echo Stopping GradLink.Api process...
taskkill /F /IM dotnet.exe /FI "WINDOWTITLE eq *GradLink.Api*" 2>nul
taskkill /F /IM GradLink.Api.exe 2>nul
echo.
echo Waiting for files to unlock...
timeout /t 3 /nobreak >nul
echo.
echo Cleaning build artifacts...
cd backend\GradLink.Api
if exist bin\Debug rmdir /s /q bin\Debug
if exist obj rmdir /s /q obj
cd ..\GradLink.Application
if exist bin\Debug rmdir /s /q bin\Debug
if exist obj rmdir /s /q obj
cd ..\GradLink.Domain
if exist bin\Debug rmdir /s /q bin\Debug
if exist obj rmdir /s /q obj
cd ..\GradLink.Infrastructure
if exist bin\Debug rmdir /s /q bin\Debug
if exist obj rmdir /s /q obj
cd ..\..
echo.
echo Building solution...
cd backend\GradLink.Api
dotnet build
echo.
echo ========================================
echo   Fix Complete!
echo ========================================
echo.
echo Now you can run: dotnet run
echo.
pause













