param(
    [String]$Configuration = "Release",
    [String]$PackageVersion = "",
    [Switch]$Install,
    [Switch]$SmokeTest
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

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
    $packArgs = @("pack", $project, "-c", $Configuration)
    if ($PackageVersion)
    {
        $packArgs += "/p:PackageVersion=$PackageVersion"
    }

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