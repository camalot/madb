# This cleans up any environment variables that may need to be reset
# this probably isn't needed, but it doesn't hurt.

if( (Test-Path -Path Env:\MADB_BUILD_VERSION) ) {
    $env:MADB_BUILD_VERSION = '';
}