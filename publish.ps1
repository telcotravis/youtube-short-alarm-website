# Publish YT Shorts Alarm website to InterServer via Web Deploy (MSDeploy).
# Usage: powershell -ExecutionPolicy Bypass -File .\publish.ps1
#
# Prerequisites:
#   - .NET 9 SDK installed
#   - Web Deploy (msdeploy.exe) installed — comes with Visual Studio or IIS
#     Download standalone: https://www.iis.net/downloads/microsoft/web-deploy

param(
    [string]$Username = "kardsenc",
    [string]$PublishUrl = "https://ytshortsalarm.kardsen.com:8172/msdeploy.axd",
    [string]$SiteName = "ytshortsalarm.kardsen.com"
)

$ErrorActionPreference = "Stop"
$projectDir = Join-Path $PSScriptRoot "YTShortsAlarm.Web"
$publishDir = Join-Path $PSScriptRoot "publish"

# --- Step 1: Build & Publish ---
Write-Host "`n=== Building and publishing ===" -ForegroundColor Cyan
dotnet publish $projectDir -c Release -o $publishDir --nologo
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "Build succeeded." -ForegroundColor Green

# --- Step 2: Find msdeploy.exe ---
$msdeployPaths = @(
    "C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe",
    "C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe",
    "${env:ProgramFiles}\IIS\Microsoft Web Deploy V3\msdeploy.exe"
)
$msdeploy = $msdeployPaths | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $msdeploy) {
    Write-Host "`nERROR: msdeploy.exe not found." -ForegroundColor Red
    Write-Host "Install Web Deploy from: https://www.iis.net/downloads/microsoft/web-deploy" -ForegroundColor Yellow
    Write-Host "`nAlternative: You can manually upload the 'publish' folder contents" -ForegroundColor Yellow
    Write-Host "  to your site via Plesk File Manager at:" -ForegroundColor Yellow
    Write-Host "  https://plesk7900.is.cc:8443" -ForegroundColor Yellow
    exit 1
}

# --- Step 3: Prompt for password ---
Write-Host "`n=== Deploying to $SiteName ===" -ForegroundColor Cyan
$securePassword = Read-Host "Enter password for $Username" -AsSecureString
$plainPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
)

# --- Step 4: Deploy via MSDeploy ---
Write-Host "Deploying..." -ForegroundColor Yellow

$msdeployArgs = @(
    "-verb:sync",
    "-source:contentPath='$publishDir'",
    "-dest:contentPath='$SiteName',computerName='$PublishUrl?site=$SiteName',userName='$Username',password='$plainPassword',authType='Basic'",
    "-allowUntrusted",
    "-enableRule:AppOffline",
    "-retryAttempts:2"
)

& $msdeploy @msdeployArgs

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nDeploy failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nDeployed successfully!" -ForegroundColor Green
Write-Host "Site: http://ytshortsalarm.kardsen.com" -ForegroundColor Cyan
