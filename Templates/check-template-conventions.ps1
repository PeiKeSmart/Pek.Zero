param(
    [String]$TemplatesRoot = $PSScriptRoot,
    [String]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent)
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-MatchValue
{
    param(
        [String]$Text,
        [String]$Pattern,
        [String]$GroupName
    )

    $match = [regex]::Match($Text, $Pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    if (!$match.Success)
    {
        return $null
    }

    return $match.Groups[$GroupName].Value.Trim()
}

$templateDefinitions = Get-ChildItem $RepositoryRoot -Recurse -File |
    Where-Object {
        $_.Name -ieq 'template.json' -and
        $_.FullName -match '[\\/]\.template\.config[\\/]template\.json$'
    }

if (!$templateDefinitions)
{
    throw "No template definitions found under: $RepositoryRoot"
}

$errors = [System.Collections.Generic.List[String]]::new()
$displayNames = [System.Collections.Generic.HashSet[String]]::new([StringComparer]::OrdinalIgnoreCase)
$shortNames = [System.Collections.Generic.HashSet[String]]::new([StringComparer]::OrdinalIgnoreCase)

foreach ($templateDefinition in $templateDefinitions)
{
    $templateText = Get-Content $templateDefinition.FullName -Raw
    $displayName = Get-MatchValue -Text $templateText -Pattern '"name"\s*:\s*"(?<value>[^"]+)"' -GroupName 'value'
    $shortName = Get-MatchValue -Text $templateText -Pattern '"shortName"\s*:\s*"(?<value>[^"]+)"' -GroupName 'value'

    if ([String]::IsNullOrWhiteSpace($displayName))
    {
        $errors.Add("Template is missing name: $($templateDefinition.FullName)")
    }
    else
    {
        if (!$displayName.StartsWith('Pek', [StringComparison]::OrdinalIgnoreCase))
        {
            $errors.Add("Template display name must start with Pek: $displayName ($($templateDefinition.FullName))")
        }

        if (!$displayNames.Add($displayName))
        {
            $errors.Add("Duplicate template display name: $displayName ($($templateDefinition.FullName))")
        }
    }

    if ([String]::IsNullOrWhiteSpace($shortName))
    {
        $errors.Add("Template is missing shortName: $($templateDefinition.FullName)")
    }
    else
    {
        if (!$shortName.StartsWith('pek', [StringComparison]::OrdinalIgnoreCase))
        {
            $errors.Add("Template shortName must start with pek: $shortName ($($templateDefinition.FullName))")
        }

        if (!$shortNames.Add($shortName))
        {
            $errors.Add("Duplicate template shortName: $shortName ($($templateDefinition.FullName))")
        }
    }
}

$templatePackages = Get-ChildItem $TemplatesRoot -Directory -Filter "*.Template.Package"
foreach ($templatePackage in $templatePackages)
{
    $readmePath = Join-Path $templatePackage.FullName "README.md"
    $packScriptPath = Join-Path $templatePackage.FullName "pack-template.ps1"
    $projectFile = Get-ChildItem $templatePackage.FullName -File -Filter "*.csproj" | Select-Object -First 1

    if (!(Test-Path $readmePath))
    {
        $errors.Add("Template package is missing README.md: $($templatePackage.FullName)")
    }

    if (!(Test-Path $packScriptPath))
    {
        $errors.Add("Template package is missing pack-template.ps1: $($templatePackage.FullName)")
    }

    if ($projectFile)
    {
        $projectText = Get-Content $projectFile.FullName -Raw
        $packageId = Get-MatchValue -Text $projectText -Pattern '<PackageId>\s*(?<value>[^<]+)\s*</PackageId>' -GroupName 'value'
        $title = Get-MatchValue -Text $projectText -Pattern '<Title>\s*(?<value>[^<]+)\s*</Title>' -GroupName 'value'

        if ([String]::IsNullOrWhiteSpace($packageId))
        {
            $errors.Add("Template package is missing PackageId: $($projectFile.FullName)")
        }
        elseif ($packageId -notmatch '^Pek[A-Za-z0-9]+\.Template$')
        {
            $errors.Add("Template package PackageId must match PekXxx.Template: $packageId ($($projectFile.FullName))")
        }

        if ([String]::IsNullOrWhiteSpace($title))
        {
            $errors.Add("Template package is missing Title: $($projectFile.FullName)")
        }
        elseif ($title -notmatch '^Pek[A-Za-z0-9]+\.Template$')
        {
            $errors.Add("Template package Title must match PekXxx.Template: $title ($($projectFile.FullName))")
        }
    }
}

$bundleProject = Join-Path (Join-Path $TemplatesRoot 'PeiKeSmart.Template.Bundle') 'PeiKeSmart.Template.Bundle.csproj'
if (Test-Path $bundleProject)
{
    $bundleText = Get-Content $bundleProject -Raw
    $bundlePackageId = Get-MatchValue -Text $bundleText -Pattern '<PackageId>\s*(?<value>[^<]+)\s*</PackageId>' -GroupName 'value'
    $bundleTitle = Get-MatchValue -Text $bundleText -Pattern '<Title>\s*(?<value>[^<]+)\s*</Title>' -GroupName 'value'

    if ($bundlePackageId -notmatch '^Pek[A-Za-z0-9]+\.Template$')
    {
        $errors.Add("Bundle PackageId must match PekXxx.Template: $bundlePackageId ($bundleProject)")
    }

    if ($bundleTitle -notmatch '^Pek[A-Za-z0-9]+\.Template$')
    {
        $errors.Add("Bundle Title must match PekXxx.Template: $bundleTitle ($bundleProject)")
    }
}

if ($errors.Count -gt 0)
{
    $errors | ForEach-Object { Write-Error $_ }
    throw "Template convention check failed. Count: $($errors.Count)"
}

Write-Host "Template convention check passed. Templates: $($templateDefinitions.Count). Packages: $($templatePackages.Count)."
$global:LASTEXITCODE = 0