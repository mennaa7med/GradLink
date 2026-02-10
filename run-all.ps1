# GradLink - Run All Services
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Starting GradLink Full Stack App" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Start .NET Backend
Write-Host "1. Starting .NET Backend API..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\backend\GradLink.Api'; Write-Host '🚀 .NET Backend API' -ForegroundColor Cyan; Write-Host 'URL: http://localhost:5000' -ForegroundColor Yellow; Write-Host 'Swagger: http://localhost:5000/swagger' -ForegroundColor Yellow; Write-Host ''; dotnet run --urls http://localhost:5000"
Start-Sleep -Seconds 2

# Start Python Flask API
Write-Host "2. Starting Python Flask API..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\Resume_Analyser_Using_Python-Main'; Write-Host '🐍 Python Flask API' -ForegroundColor Cyan; Write-Host 'For Resume Analysis' -ForegroundColor Yellow; Write-Host ''; if (Test-Path '..\venv\Scripts\Activate.ps1') { & '..\venv\Scripts\Activate.ps1' }; python main.py"
Start-Sleep -Seconds 2

# Start Vite Frontend
Write-Host "3. Starting Vite Frontend..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot'; Write-Host '⚡ Vite React Frontend' -ForegroundColor Cyan; Write-Host 'URL: http://localhost:5173' -ForegroundColor Yellow; Write-Host ''; npm run dev"

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  ✅ All Services Started!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Access your app at:" -ForegroundColor Cyan
Write-Host "  🌐 Frontend:  http://localhost:5173" -ForegroundColor White
Write-Host "  📚 Backend:   http://localhost:5000/swagger" -ForegroundColor White
Write-Host ""
Write-Host "Three PowerShell windows have opened." -ForegroundColor Yellow
Write-Host "Close them or press Ctrl+C to stop services." -ForegroundColor Yellow
Write-Host ""

