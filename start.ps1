# GradLink Full-Stack Startup Script
# This script sets up environment variables and starts Docker Compose

Write-Host "🚀 GradLink Full-Stack Startup" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Check if Docker is running
try {
    docker info | Out-Null
    Write-Host "✅ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker is not running. Please start Docker Desktop first." -ForegroundColor Red
    exit 1
}

# Set default environment variables if not already set
if (-not $env:JWT_SECRET) {
    $env:JWT_SECRET = "GradLink_Dev_Secret_Key_32_Chars_Minimum!"
    Write-Host "📝 Using default JWT_SECRET" -ForegroundColor Yellow
}

if (-not $env:ADMIN_EMAIL) {
    $env:ADMIN_EMAIL = "admin@example.com"
    Write-Host "📝 Using default ADMIN_EMAIL: $env:ADMIN_EMAIL" -ForegroundColor Yellow
}

if (-not $env:ADMIN_PASSWORD) {
    $env:ADMIN_PASSWORD = "P@ssw0rd!"
    Write-Host "📝 Using default ADMIN_PASSWORD" -ForegroundColor Yellow
}

if (-not $env:VITE_API_BASE_URL) {
    $env:VITE_API_BASE_URL = "http://localhost:8080"
    Write-Host "📝 Using default VITE_API_BASE_URL: $env:VITE_API_BASE_URL" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Environment Variables:" -ForegroundColor Cyan
Write-Host "  JWT_SECRET: $($env:JWT_SECRET.Substring(0, [Math]::Min(20, $env:JWT_SECRET.Length)))..." -ForegroundColor Gray
Write-Host "  ADMIN_EMAIL: $env:ADMIN_EMAIL" -ForegroundColor Gray
Write-Host "  ADMIN_PASSWORD: [HIDDEN]" -ForegroundColor Gray
Write-Host "  VITE_API_BASE_URL: $env:VITE_API_BASE_URL" -ForegroundColor Gray
if ($env:GEMINI_API_KEY) {
    Write-Host "  GEMINI_API_KEY: [SET]" -ForegroundColor Gray
} else {
    Write-Host "  GEMINI_API_KEY: [NOT SET - Gemini chat will not work]" -ForegroundColor Yellow
}
Write-Host ""

Write-Host "🔨 Building and starting containers..." -ForegroundColor Cyan
Write-Host ""

# Build and start Docker Compose
docker compose up --build

Write-Host ""
Write-Host "✅ GradLink is running!" -ForegroundColor Green
Write-Host ""
Write-Host "Access points:" -ForegroundColor Cyan
Write-Host "  🌐 Web App:    http://localhost:8081" -ForegroundColor White
Write-Host "  📚 API Swagger: http://localhost:8080/swagger" -ForegroundColor White
Write-Host ""
Write-Host "Default Admin Login:" -ForegroundColor Cyan
Write-Host "  Email:    $env:ADMIN_EMAIL" -ForegroundColor White
Write-Host "  Password: $env:ADMIN_PASSWORD" -ForegroundColor White
Write-Host ""
Write-Host "Press Ctrl+C to stop all services" -ForegroundColor Yellow

