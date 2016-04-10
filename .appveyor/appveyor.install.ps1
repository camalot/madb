& choco install -y nuget.commandline -version 3.3.0;

$env:PATH = "C:\ProgramData\chocolatey\lib\NuGet.CommandLine\tools\;C:\Python34;C:\Python34\Scripts\;$env:PATH";

& python -m pip install --upgrade pip;