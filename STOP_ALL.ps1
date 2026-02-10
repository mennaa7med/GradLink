# Stop All GradLink Services - PowerShell Script

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Red
Write-Host "        🛑 Stopping All GradLink Services" -ForegroundColor Yellow
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Red
Write-Host ""

# Function to kill process on port
function Kill-Port {
    param([int]$Port, [string]$ServiceName)
    
    Write-Host "[$ServiceName] Checking port $Port..." -ForegroundColor Yellow
    
    $connections = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
    
    if ($connections) {
        foreach ($conn in $connections) {
            $process = Get-Process -Id $conn.OwningProcess -ErrorAction SilentlyContinue
            if ($process) {
                Write-Host "  Found: $($process.ProcessName) (PID: $($process.Id))" -ForegroundColor Cyan
                Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
                Write-Host "  ✅ Stopped" -ForegroundColor Green
            }
        }
    } else {
        Write-Host "  ℹ️  No process on port $Port" -ForegroundColor Gray
    }
}

# Kill services by port
Kill-Port -Port 5173 -ServiceName "Frontend (Vite)"
Kill-Port -Port 5000 -ServiceName "Backend (.NET)"
Kill-Port -Port 5005 -ServiceName "Flask API"

Write-Host ""
Write-Host "Stopping remaining Node/Python/Dotnet processes..." -ForegroundColor Yellow

# Kill by process name
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process -Name "python" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Red
Write-Host "             ✅ All Services Stopped!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Red
Write-Host ""

# Show current port status
Write-Host "Current port status:" -ForegroundColor Cyan
Write-Host ""

$ports = @(5173, 5000, 5005)
$allFree = $true

foreach ($port in $ports) {
    $conn = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($conn) {
        Write-Host "  Port $port : ⚠️  Still in use" -ForegroundColor Yellow
        $allFree = $false
    } else {
        Write-Host "  Port $port : ✅ Free" -ForegroundColor Green
    }
}

Write-Host ""
if ($allFree) {
    Write-Host "✅ All ports are now free!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Some ports may still be in use. Try running again." -ForegroundColor Yellow
}

Write-Host ""
pause

