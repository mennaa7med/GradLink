@echo off
chcp 65001 >nul
cls
title GradLink - Starting All Services
echo ════════════════════════════════════════════════════════
echo   Starting GradLink Project - All Services
echo ════════════════════════════════════════════════════════
echo.

echo [1/3] Backend (.NET) on port 5000...
start "Backend API" cmd /k "cd backend\GradLink.Api && dotnet run --urls http://localhost:5000"
timeout /t 2 /nobreak >nul

echo [2/3] Flask API on port 5005...
start "Flask API" cmd /k "cd Resume_Analyser_Using_Python-Main && python main.py"
timeout /t 2 /nobreak >nul

echo [3/3] Frontend on port 5173...
start "Frontend" cmd /k "npm run dev"

echo.
echo ════════════════════════════════════════════════════════
echo ✅ All services started!
echo.
echo Access:
echo   Frontend:  http://localhost:5173
echo   Backend:   http://localhost:5000
echo   Flask API: http://localhost:5005
echo.
echo Close windows to stop.
echo ════════════════════════════════════════════════════════
pause

