@echo off
chcp 65001 >nul
cls
color 0A
echo ════════════════════════════════════════════════════════
echo           🚀 GradLink - Starting All Services
echo ════════════════════════════════════════════════════════
echo.

REM Check Python
echo [Check] Verifying Python installation...
python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Python is not installed or not in PATH
    echo    Please install Python 3.7+ from python.org
    pause
    exit /b 1
)
echo ✅ Python is installed
echo.

REM Install Flask dependencies (silent, only if needed)
echo [Setup] Installing Flask dependencies...
cd Resume_Analyser_Using_Python-Main
python -m pip install --quiet --disable-pip-version-check PyPDF2 flask flask-cors google-generativeai python-dotenv 2>nul
cd ..
echo ✅ Flask dependencies ready
echo.

REM Start Backend (.NET)
echo [1/3] Starting .NET Backend API...
start "🔧 GradLink Backend API" cmd /k "cd backend\GradLink.Api && dotnet run --urls http://localhost:5000"
timeout /t 3 /nobreak >nul
echo ✅ Backend starting...
echo.

REM Start Flask API
echo [2/3] Starting Flask API (Resume Analyzer)...
start "🐍 Flask Resume API" cmd /k "cd Resume_Analyser_Using_Python-Main && python main.py"
timeout /t 3 /nobreak >nul
echo ✅ Flask API starting...
echo.

REM Start Frontend
echo [3/3] Starting Vite Frontend...
start "⚛️ Vite Frontend" cmd /k "npm run dev"
timeout /t 3 /nobreak >nul
echo ✅ Frontend starting...
echo.

echo ════════════════════════════════════════════════════════
echo              ✅ All Services Started!
echo ════════════════════════════════════════════════════════
echo.
echo 📱 Frontend:        http://localhost:5173
echo 🔧 .NET Backend:    http://localhost:5000
echo 📚 Swagger UI:      http://localhost:5000/swagger
echo 🐍 Flask API:       http://localhost:5005
echo.
echo ════════════════════════════════════════════════════════
echo 💡 Tips:
echo    - Wait 10-15 seconds for all services to be ready
echo    - Check each service window for any errors
echo    - Close windows to stop services
echo ════════════════════════════════════════════════════════
echo.
pause

