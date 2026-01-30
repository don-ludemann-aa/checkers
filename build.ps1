$ErrorActionPreference = 'Stop'

Write-Host "Restoring..."
dotnet restore

Write-Host "Building..."
dotnet build -c Release

Write-Host "Testing..."
dotnet test -c Release
