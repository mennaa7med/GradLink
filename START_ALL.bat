@echo off
echo ====================================
echo Starting GradLink - All Services
echo ====================================
echo.

echo [1/3] Starting .NET Backend API...
start "GradLink Backend API" cmd /k "cd backend\GradLink.Api && dotnet run --urls http://localhost:5000"

timeout /t 3 /nobreak >nul

echo [2/3] Starting Python Flask API (Resume Analyzer)...
start "Flask Resume API" cmd /k "cd Resume_Analyser_Using_Python-Main && python main.py"

timeout /t 3 /nobreak >nul

echo [3/3] Starting Vite Frontend...
start "Vite Frontend" cmd /k "npm run dev"

echo.
echo ====================================
echo All services are starting!
echo ====================================
echo.
echo Frontend:  http://localhost:5173
echo Backend:   http://localhost:5000
echo Swagger:   http://localhost:5000/swagger
echo Flask API: http://localhost:5005 (Resume Analyzer)
echo.
echo Close the opened windows to stop the services.
echo.
pause

