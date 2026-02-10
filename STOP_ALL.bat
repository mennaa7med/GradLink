@echo off
chcp 65001 >nul
cls
color 0C
echo ════════════════════════════════════════════════════════
echo   🛑 Stopping All GradLink Services
echo ════════════════════════════════════════════════════════
echo.

echo [1/4] Stopping Vite Frontend (port 5173)...
for /f "tokens=5" %%a in ('netstat -aon ^| find ":5173" ^| find "LISTENING"') do (
    taskkill /F /PID %%a >nul 2>&1
    if !errorlevel! equ 0 echo ✅ Frontend stopped
)

echo [2/4] Stopping .NET Backend (port 5000)...
for /f "tokens=5" %%a in ('netstat -aon ^| find ":5000" ^| find "LISTENING"') do (
    taskkill /F /PID %%a >nul 2>&1
    if !errorlevel! equ 0 echo ✅ Backend stopped
)

echo [3/4] Stopping Flask API (port 5005)...
for /f "tokens=5" %%a in ('netstat -aon ^| find ":5005" ^| find "LISTENING"') do (
    taskkill /F /PID %%a >nul 2>&1
    if !errorlevel! equ 0 echo ✅ Flask API stopped
)

echo [4/4] Stopping Node processes...
taskkill /F /IM node.exe >nul 2>&1
taskkill /F /IM python.exe >nul 2>&1
taskkill /F /IM dotnet.exe >nul 2>&1

echo.
echo ════════════════════════════════════════════════════════
echo   ✅ All services stopped!
echo ════════════════════════════════════════════════════════
echo.
pause

