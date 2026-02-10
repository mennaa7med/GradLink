@echo off
chcp 65001 >nul
cls
title Kill All Ports - GradLink
color 0C

echo.
echo ════════════════════════════════════════════════════════
echo   🛑 Killing All Processes on Ports
echo ════════════════════════════════════════════════════════
echo.

REM Kill port 5173 (Frontend)
echo [Port 5173] Killing Frontend...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :5173') do (
    echo   Found PID: %%a
    taskkill /F /PID %%a 2>nul
)

REM Kill port 5000 (Backend)
echo [Port 5000] Killing Backend...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :5000') do (
    echo   Found PID: %%a
    taskkill /F /PID %%a 2>nul
)

REM Kill port 5005 (Flask)
echo [Port 5005] Killing Flask API...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :5005') do (
    echo   Found PID: %%a
    taskkill /F /PID %%a 2>nul
)

echo.
echo ════════════════════════════════════════════════════════
echo   ✅ All ports cleared!
echo ════════════════════════════════════════════════════════
echo.

REM Show remaining processes
echo Current port status:
echo.
netstat -aon | findstr ":5173 :5000 :5005"
if errorlevel 1 (
    echo   ✅ All ports are free
) else (
    echo   ⚠️  Some ports still in use
)

echo.
pause

