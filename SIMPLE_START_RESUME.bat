@echo off
chcp 65001
cls
echo ========================================
echo   Starting Resume Analyser (Flask API)
echo ========================================
echo.

REM Change to script directory
cd /d "%~dp0"

REM Navigate to Resume Analyser folder
cd Resume_Analyser_Using_Python-Main

echo [Step 1] Installing dependencies...
python -m pip install --quiet flask flask-cors PyPDF2 google-generativeai python-dotenv

echo.
echo [Step 2] Starting Flask API on http://localhost:5005
echo.
echo ========================================
echo   Flask API will run on port 5005
echo   Press Ctrl+C to stop
echo ========================================
echo.

python main.py

pause

