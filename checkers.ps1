$ErrorActionPreference = 'Stop'

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionDir = $scriptDir
$projectPath = Join-Path $solutionDir 'src/Checkers.Cli/Checkers.Cli.csproj'

Write-Host "Running Checkers CLI..."
dotnet run --project $projectPath
