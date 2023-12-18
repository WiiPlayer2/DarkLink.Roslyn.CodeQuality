if ($env:Path -notcontains "Community\MSBuild\Current\Bin\amd64") {
    $env:Path = "$env:Path;C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64"
}

Push-Location $PSScriptRoot

New-Item -ItemType Directory ./out -ErrorAction SilentlyContinue | Out-Null

msbuild `
    -property:Configuration=Release `
    -property:OutDir=$((Get-Item ./out).FullName)/bin `
    ./DarkLink.Roslyn.CodeQuality/DarkLink.Roslyn.CodeQuality.Vsix/DarkLink.Roslyn.CodeQuality.Vsix.csproj

Copy-Item ./out/bin/DarkLink.Roslyn.CodeQuality.Vsix.vsix ./out/

Pop-Location

