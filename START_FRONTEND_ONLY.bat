@echo off
echo ====================================
echo Starting Frontend ONLY
echo ====================================
echo.

cd /d "%~dp0"

echo Current directory: %cd%
echo.

echo Checking Node.js...
node --version
npm --version
echo.

echo Starting Vite Frontend on Port 5173...
echo.

npm run dev

pause

