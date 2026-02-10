@echo off
chcp 65001 >nul
echo ========================================
echo   Resume Analyser - Flask API Starter
echo ========================================
echo.

cd /d "%~dp0Resume_Analyser_Using_Python-Main"

echo [1/3] Checking Python...
python --version
if errorlevel 1 (
    echo ERROR: Python is not installed or not in PATH
    pause
    exit /b 1
)
echo.

echo [2/3] Installing dependencies...
python -m pip install -r requirements.txt
if errorlevel 1 (
    echo ERROR: Failed to install dependencies
    pause
    exit /b 1
)
echo.

echo [3/3] Starting Flask API on port 5005...
echo.
echo ========================================
echo   API will be available at:
echo   http://localhost:5005
echo ========================================
echo.
echo Press Ctrl+C to stop the server
echo.

python main.py

pause

