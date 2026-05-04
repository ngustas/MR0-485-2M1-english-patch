param(
    [string]$Runtime = "win-x64",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$project = Join-Path $repoRoot "TranslatorTools\\TranslatorTools.csproj"
$outputDir = Join-Path $repoRoot "dist\\MR0-485-2M1-patcher-$Runtime"

if (Get-Command dotnet -ErrorAction SilentlyContinue) {
    $dotnet = "dotnet"
} elseif (Test-Path "C:\Program Files\dotnet\dotnet.exe") {
    $dotnet = "C:\Program Files\dotnet\dotnet.exe"
} else {
    throw "dotnet was not found. Install the .NET 8 SDK first."
}

& $dotnet publish $project `
    -c $Configuration `
    -r $Runtime `
    --self-contained false `
    /p:PublishSingleFile=true `
    -o $outputDir

$publishedExe = Join-Path $outputDir "TranslatorTools.exe"
$finalExe = Join-Path $outputDir "MR0-485-2M1-patcher.exe"

if (Test-Path $publishedExe) {
    Copy-Item -LiteralPath $publishedExe -Destination $finalExe -Force
}

Write-Host "Published framework-dependent patcher to $finalExe"
Write-Host "Requires .NET 8 Desktop Runtime x64 on the target machine."
