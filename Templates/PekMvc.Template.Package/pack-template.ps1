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
    $project = Join-Path $PSScriptRoot "PekMvc.Template.Package.csproj"
    $packArgs = @("pack", $project, "-c", $Configuration)
    if ($PackageVersion)
    {
        $packArgs += "/p:PackageVersion=$PackageVersion"
    }

    dotnet @packArgs
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $nupkg = Get-ChildItem (Join-Path $PSScriptRoot "bin\$Configuration\PekMvc.Template.*.nupkg") |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1 -ExpandProperty FullName

    Write-Host "模板包: $nupkg"

    if ($Install -or $SmokeTest)
    {
        try
        {
            dotnet new uninstall PekMvc.Template *>$null
        }
        catch
        {
            $LASTEXITCODE = 0
        }
        dotnet new install $nupkg
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }

    if ($SmokeTest)
    {
        $tempRoot = Join-Path $env:TEMP "PekMvc.Template.Smoke"
        if (Test-Path $tempRoot)
        {
            Remove-Item $tempRoot -Recurse -Force
        }

        $projectOut = Join-Path $tempRoot "project"
        $solutionOut = Join-Path $tempRoot "solution"

        dotnet new pekmvc -n DemoPekMvc -o $projectOut -P Demo站点 -Pr Demo项目描述 -C DemoCompany -S DemoService -D DemoDb
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        dotnet build (Join-Path $projectOut "DemoPekMvc.csproj") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

        dotnet new pekmvc-sln -n DemoPekMvcSolution -o $solutionOut -P Demo站点 -Pr Demo项目描述 -C DemoCompany -S DemoService -D DemoDb
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        dotnet build (Join-Path $solutionOut "Mvc\DemoPekMvcSolution\DemoPekMvcSolution.csproj") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }
}
finally
{
    Pop-Location
}