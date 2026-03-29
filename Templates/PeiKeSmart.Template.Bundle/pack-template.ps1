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
    $project = Join-Path $PSScriptRoot "PeiKeSmart.Template.Bundle.csproj"
    $templatePackageIds = @("PekBundle.Template", "PeiKeSmart.Template.Bundle", "PekVueZero.Template", "VueZero.Template")
    $templatesRoot = Join-Path $PSScriptRoot ".."
    $checkScript = Join-Path $templatesRoot "check-template-conventions.ps1"

    & $checkScript -TemplatesRoot $templatesRoot
    if (!$?) { exit 1 }

    $packageProjects = Get-ChildItem $templatesRoot -Directory -Filter "*.Template.Package" |
        ForEach-Object { Get-ChildItem $_.FullName -File -Filter "*.csproj" | Select-Object -First 1 }

    foreach ($packageProject in $packageProjects)
    {
        $projectText = Get-Content $packageProject.FullName -Raw
        $packageIdMatch = [regex]::Match($projectText, '<PackageId>\s*(?<id>[^<]+?)\s*</PackageId>', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
        if ($packageIdMatch.Success)
        {
            $packageId = $packageIdMatch.Groups['id'].Value.Trim()
            if (![String]::IsNullOrWhiteSpace($packageId))
            {
                $templatePackageIds += $packageId
            }
        }
    }

    $templatePackageIds = $templatePackageIds | Select-Object -Unique
    $packArgs = @("pack", $project, "-c", $Configuration)
    if ($PackageVersion)
    {
        $packArgs += "/p:PackageVersion=$PackageVersion"
    }

    & dotnet @packArgs
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $nupkg = Get-ChildItem (Join-Path $PSScriptRoot "bin\$Configuration\PekBundle.Template.*.nupkg") |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1 -ExpandProperty FullName

    Write-Host "模板包: $nupkg"

    if ($Install -or $SmokeTest)
    {
        foreach ($templatePackageId in $templatePackageIds)
        {
            try
            {
                & dotnet new uninstall $templatePackageId *>$null
            }
            catch
            {
                $LASTEXITCODE = 0
            }
        }

        & dotnet new install $nupkg
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }

    if ($SmokeTest)
    {
        $tempRoot = Join-Path $env:TEMP "PekBundle.Template.Smoke"
        if (Test-Path $tempRoot)
        {
            Remove-Item $tempRoot -Recurse -Force
        }

        $pekProjectOut = Join-Path $tempRoot "PekMvc.Project"
        $pekSolutionOut = Join-Path $tempRoot "PekMvc.Solution"
        $vueProjectOut = Join-Path $tempRoot "VueZero.Project"
        $vueSolutionOut = Join-Path $tempRoot "VueZero.Solution"

        & dotnet new pekmvc -n DemoPekMvc -o $pekProjectOut -P Demo站点 -Pr Demo项目描述 -C DemoCompany -S DemoService -D DemoDb
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        & dotnet build (Join-Path $pekProjectOut "DemoPekMvc.csproj") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

        & dotnet new pekmvc-sln -n DemoPekMvcSolution -o $pekSolutionOut -P Demo站点 -Pr Demo项目描述 -C DemoCompany -S DemoService -D DemoDb
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        & dotnet build (Join-Path $pekSolutionOut "Mvc\DemoPekMvcSolution\DemoPekMvcSolution.csproj") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

        $vueProjectArgs = @(
            'new', 'pekvuezero',
            '-n', 'DemoPekVueZero',
            '-o', $vueProjectOut,
            '--ProjectTitle', 'DemoApp',
            '--ProjectDescription', 'Demo full stack template',
            '--CompanyName', 'DemoCompany',
            '--ServiceName', 'DemoPekVueZero.Service',
            '--ServerHttpPort', '5217',
            '--ServerHttpsPort', '7372',
            '--ClientDevPort', '53017',
            '--ClientPackageName', 'demo.client'
        )
        & dotnet @vueProjectArgs
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        & dotnet build (Join-Path $vueProjectOut "DemoPekVueZero.Server\DemoPekVueZero.Server.csproj") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

        $vueSolutionArgs = @(
            'new', 'pekvuezero-sln',
            '-n', 'DemoPekVueZero',
            '-o', $vueSolutionOut,
            '--ProjectTitle', 'DemoApp',
            '--ProjectDescription', 'Demo full stack template',
            '--CompanyName', 'DemoCompany',
            '--ServiceName', 'DemoPekVueZero.Service',
            '--ServerHttpPort', '5217',
            '--ServerHttpsPort', '7372',
            '--ClientDevPort', '53017',
            '--ClientPackageName', 'demo.client'
        )
        & dotnet @vueSolutionArgs
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        & dotnet build (Join-Path $vueSolutionOut "DemoPekVueZero.slnx") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }
}
finally
{
    Pop-Location
}