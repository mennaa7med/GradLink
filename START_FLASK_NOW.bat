@echo off
chcp 65001 >nul
cls
echo ============================================
echo   🚀 Starting Flask Resume Analyzer API
echo ============================================
echo.

cd /d "%~dp0Resume_Analyser_Using_Python-Main"

echo [1/2] Installing PyPDF2...
python -m pip install --quiet PyPDF2 flask flask-cors google-generativeai python-dotenv

echo.
echo [2/2] Starting Flask API on http://localhost:5005
echo.
echo ============================================
echo   ✅ Flask API: http://localhost:5005
echo   📡 Endpoint: /api/analyze (POST)
echo   🛑 Press Ctrl+C to stop
echo ============================================
echo.

python main.py

