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
    $project = Join-Path $PSScriptRoot "VueZero.Template.Package.csproj"
    $packArgs = @("pack", $project, "-c", $Configuration)
    if ($PackageVersion)
    {
        $packArgs += "/p:PackageVersion=$PackageVersion"
    }

    & dotnet @packArgs
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $nupkg = Get-ChildItem (Join-Path $PSScriptRoot "bin\$Configuration\VueZero.Template.*.nupkg") |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1 -ExpandProperty FullName

    Write-Host "模板包: $nupkg"

    if ($Install -or $SmokeTest)
    {
        & dotnet new uninstall VueZero.Template | Out-Null
        & dotnet new install $nupkg
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }

    if ($SmokeTest)
    {
        $tempRoot = Join-Path $env:TEMP "VueZero.Template.Smoke"
        if (Test-Path $tempRoot)
        {
            Remove-Item $tempRoot -Recurse -Force
        }

        $projectOut = Join-Path $tempRoot "project"
        $solutionOut = Join-Path $tempRoot "solution"

        $projectArgs = @(
            'new', 'vuezero',
            '-n', 'DemoVueZero',
            '-o', $projectOut,
            '--ProjectTitle', 'DemoApp',
            '--ProjectDescription', 'Demo full stack template',
            '--CompanyName', 'DemoCompany',
            '--ServiceName', 'DemoVueZero.Service',
            '--ServerHttpPort', '5217',
            '--ServerHttpsPort', '7372',
            '--ClientDevPort', '53017',
            '--ClientPackageName', 'demo.client'
        )
        & dotnet @projectArgs
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        & dotnet build (Join-Path $projectOut "DemoVueZero.Server\DemoVueZero.Server.csproj") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

        $solutionArgs = @(
            'new', 'vuezero-sln',
            '-n', 'DemoVueZero',
            '-o', $solutionOut,
            '--ProjectTitle', 'DemoApp',
            '--ProjectDescription', 'Demo full stack template',
            '--CompanyName', 'DemoCompany',
            '--ServiceName', 'DemoVueZero.Service',
            '--ServerHttpPort', '5217',
            '--ServerHttpsPort', '7372',
            '--ClientDevPort', '53017',
            '--ClientPackageName', 'demo.client'
        )
        & dotnet @solutionArgs
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
        & dotnet build (Join-Path $solutionOut "DemoVueZero.slnx") -nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }
}
finally
{
    Pop-Location
}