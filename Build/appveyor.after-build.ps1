if( !(Test-Path -Path Env:\MADB_BUILD_VERSION) -and (Test-Path -Path .\VersionAssemblyInfo.txt) ) {
    $version = (Get-Content -Path .\VersionAssemblyInfo.txt)
    $env:MADB_BUILD_VERSION = $version;
}