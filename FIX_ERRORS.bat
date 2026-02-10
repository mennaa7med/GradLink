@echo off
echo ========================================
echo   Fix React HMR and CSP Errors
echo ========================================
echo.
echo Stopping any running dev servers...
taskkill /F /IM node.exe 2>nul
echo.
echo Clearing npm cache...
npm cache clean --force
echo.
echo Removing node_modules and reinstalling...
rmdir /s /q node_modules 2>nul
del package-lock.json 2>nul
npm install
echo.
echo Clearing Vite cache...
rmdir /s /q node_modules\.vite 2>nul
echo.
echo ========================================
echo   Setup Complete!
echo ========================================
echo.
echo Now run: npm run dev
echo.
echo If error persists:
echo 1. Close browser completely
echo 2. Open browser in Incognito mode
echo 3. Go to http://localhost:5176
echo.
pause













