try {
	& nuget restore -NonInteractive;
} catch {
	$_.Exception.Message | Write-Error;
}

Import-Module "$env:APPVEYOR_BUILD_FOLDER\.appveyor\modules\Import-PfxCertificate.psm1";
Import-Module "$env:APPVEYOR_BUILD_FOLDER\.appveyor\modules\Set-BuildVersion.psm1";


Import-PfxCertificate -pfx "$env:APPVEYOR_BUILD_FOLDER\Shared\madb.pfx" -password ((Get-Item Env:\MADB_PFX_KEY).Value) -containerName ((Get-Item Env:\VS_PFX_KEY).Value);

$env:CI_BUILD_DATE = ((Get-Date).ToUniversalTime().ToString("MM-dd-yyyy"));
$env:CI_BUILD_TIME = ((Get-Date).ToUniversalTime().ToString("hh:mm:ss"));

Set-BuildVersion;

