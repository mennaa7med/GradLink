$ErrorActionPreference = "Stop"

# Paths
$apiProj = "backend\Api\GradLink.Api.csproj"
$webDir = "."

# Env defaults (can be overridden in current shell)
if (-not $env:JWT_SECRET) { $env:JWT_SECRET = "CHANGE_ME_DEV_SECRET_32CHARS_MINIMUM____________" }
if (-not $env:Admin__Email) { $env:Admin__Email = "admin@example.com" }
if (-not $env:Admin__Password) { $env:Admin__Password = "P@ssw0rd!" }
if (-not $env:VITE_API_BASE_URL) { $env:VITE_API_BASE_URL = "http://localhost:5080" }

Write-Host "Starting GradLink dev environment..." -ForegroundColor Cyan
Write-Host "API base: $($env:VITE_API_BASE_URL)"

# Start API
Write-Host "Starting .NET API..." -ForegroundColor Green
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project `"$apiProj`""

Start-Sleep -Seconds 2

# Start Vite
Write-Host "Starting Vite frontend..." -ForegroundColor Green
Push-Location $webDir
if (-not (Test-Path "node_modules")) {
    Write-Host "Installing npm dependencies..."
    npm ci
}
Start-Process -NoNewWindow -FilePath "npm" -ArgumentList "run dev"
Pop-Location

Write-Host ""
Write-Host "GradLink dev environment started." -ForegroundColor Cyan
Write-Host "API:    http://localhost:5080 (Kestrel will choose port if not overridden)"
Write-Host "Web:    http://localhost:5173"
Write-Host "Notes:"
Write-Host " - Admin seeding via Admin__Email and Admin__Password env vars"
Write-Host " - VITE_API_BASE_URL used by frontend for API calls"
Write-Host " - Set GEMINI_API_KEY to enable Gemini proxy"

