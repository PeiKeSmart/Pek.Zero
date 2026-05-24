param(
    [String]$Configuration = "Release",
    [String]$PackageVersion = "",
    [Switch]$Install,
    [Switch]$SmokeTest
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-DefaultPackageVersion([String]$projectPath)
{
    $projectText = Get-Content $projectPath -Raw
    $versionMatch = [regex]::Match($projectText, '<PackageVersion>\s*(?<version>[^<]+)\s*</PackageVersion>', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    if (!$versionMatch.Success)
    {
        throw "PackageVersion was not found in $projectPath"
    }

    $currentVersion = $versionMatch.Groups['version'].Value.Trim()
    $prefixMatch = [regex]::Match($currentVersion, '^(?<prefix>\d+\.\d+)\.\d{4}\.\d{4}(-beta\d{4})?$')
    if (!$prefixMatch.Success)
    {
        throw "Unexpected PackageVersion format: $currentVersion"
    }

    $chinaTimeZone = [System.TimeZoneInfo]::FindSystemTimeZoneById('China Standard Time')
    $now = [System.TimeZoneInfo]::ConvertTime([DateTimeOffset]::UtcNow, $chinaTimeZone)
    return '{0}.{1}' -f $prefixMatch.Groups['prefix'].Value, $now.ToString('yyyy.MMdd')
}

Push-Location $PSScriptRoot
try
{
    if ($Install -or $SmokeTest)
    {
        throw "Direct install and SmokeTest are disabled for PekMvc.Template. Use Templates\\PeiKeSmart.Template.Bundle\\pack-template.ps1 -Install or -Install -SmokeTest instead."
    }

    $templatesRoot = Join-Path $PSScriptRoot ".."
    $checkScript = Join-Path $templatesRoot "check-template-conventions.ps1"
    & $checkScript -TemplatesRoot $templatesRoot
    if (!$?) { exit 1 }

    $project = Join-Path $PSScriptRoot "PekMvc.Template.Package.csproj"
    $resolvedPackageVersion = $PackageVersion
    if ([String]::IsNullOrWhiteSpace($resolvedPackageVersion))
    {
        $resolvedPackageVersion = Get-DefaultPackageVersion $project
    }

    $packArgs = @("pack", $project, "-c", $Configuration)
    $packArgs += "/p:PackageVersion=$resolvedPackageVersion"

    Write-Host "PackageVersion: $resolvedPackageVersion"

    dotnet @packArgs
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $packageOutputRoot = Join-Path (Join-Path $PSScriptRoot "bin") $Configuration
    $nupkg = Get-ChildItem (Join-Path $packageOutputRoot "PekMvc.Template.*.nupkg") |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1 -ExpandProperty FullName

    Write-Host "Package: $nupkg"

}
finally
{
    Pop-Location
}