# GradLink - Start All Services
# PowerShell Script

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "        GradLink - Starting All Services" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# Check Python
Write-Host "[Check] Verifying Python..." -ForegroundColor Yellow
try {
    $pythonVersion = python --version 2>&1
    Write-Host "✅ $pythonVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Python not found. Please install Python 3.7+" -ForegroundColor Red
    pause
    exit 1
}
Write-Host ""

# Install Flask dependencies
Write-Host "[Setup] Installing Flask dependencies..." -ForegroundColor Yellow
Set-Location "Resume_Analyser_Using_Python-Main"
python -m pip install --quiet --disable-pip-version-check PyPDF2 flask flask-cors google-generativeai python-dotenv 2>$null
Set-Location ".."
Write-Host "✅ Flask dependencies ready" -ForegroundColor Green
Write-Host ""

# Start Backend (.NET)
Write-Host "[1/3] Starting .NET Backend API..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'backend\GradLink.Api'; dotnet run --urls http://localhost:5000" -WindowStyle Normal
Start-Sleep -Seconds 2
Write-Host "✅ Backend starting on port 5000" -ForegroundColor Green
Write-Host ""

# Start Flask API
Write-Host "[2/3] Starting Flask API..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Resume_Analyser_Using_Python-Main'; python main.py" -WindowStyle Normal
Start-Sleep -Seconds 2
Write-Host "✅ Flask API starting on port 5005" -ForegroundColor Green
Write-Host ""

# Start Frontend
Write-Host "[3/3] Starting Vite Frontend..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "npm run dev" -WindowStyle Normal
Start-Sleep -Seconds 2
Write-Host "✅ Frontend starting on port 5173" -ForegroundColor Green
Write-Host ""

Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "             ✅ All Services Started!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
Write-Host "📱 Frontend:        http://localhost:5173" -ForegroundColor White
Write-Host "🔧 .NET Backend:    http://localhost:5000" -ForegroundColor White
Write-Host "📚 Swagger UI:      http://localhost:5000/swagger" -ForegroundColor White
Write-Host "🐍 Flask API:       http://localhost:5005" -ForegroundColor White
Write-Host ""
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "💡 Wait 10-15 seconds for all services to be ready" -ForegroundColor Yellow
Write-Host "💡 Close PowerShell windows to stop services" -ForegroundColor Yellow
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
pause
