@echo off
title Flask Resume Analyzer API
color 0A
cls
echo.
echo   ============================================
echo      Flask Resume Analyzer API
echo   ============================================
echo.
echo   Starting Flask API on http://localhost:5005
echo.
echo   Press Ctrl+C to stop
echo   ============================================
echo.

cd /d "%~dp0Resume_Analyser_Using_Python-Main"
python main.py

pause

