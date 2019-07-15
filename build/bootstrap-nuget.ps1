# Check for nuget exe. If it doesn't exist, create full path to parent, and download it
function BootStrap-Nuget {
    [cmdletbinding()]
    param(
        $NugetPath = "$env:APPDATA\nuget.exe"
    )

    if($c = Get-Command 'nuget.exe' -ErrorAction SilentlyContinue)
    {
        write-verbose "Found Nuget at [$($c.path)]"
        return
    }

    #Don't have it, download it
    $Parent = Split-Path $NugetPath -Parent
    if(-not (Test-Path $NugetPath))
    {
        if(-not (Test-Path $Parent))
        {
            Write-Verbose "Creating parent paths to [$NugetPath]'s parent: [$Parent]"
            mkdir $Parent -Force   
        }
        Write-Verbose "Downloading nuget to [$NugetPath]"
        Invoke-WebRequest -uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile $NugetPath
    }

    # Add to path
    if( ($ENV:Path -split ';') -notcontains $Parent )
    {
        $ENV:Path = $ENV:Path, $Parent -join ';'
    }
}

BootStrap-Nuget -Verbose