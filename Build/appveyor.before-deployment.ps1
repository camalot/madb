<#
 #
 #
 #>

$commitMessageRegex = "^\[deploy\:(pre-release|draft|release)\]";

# Must come from master branch.
# Must not have a PULL Request Number
# Must match regex


Write-Host "$env:APPVEYOR_REPO_COMMIT_MESSAGE -match $commitMessageRegex";
Write-Host "Branch: $env:APPVEYOR_REPO_BRANCH";

if ( ($env:APPVEYOR_REPO_BRANCH -eq "master") -and ($env:APPVEYOR_REPO_COMMIT_MESSAGE -match $commitMessageRegex) ) {
	$env:CI_RELEASE_DESCRIPTION = $env:APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED
	$env:CI_DEPLOY_NUGET = $true;
  $env:CI_DEPLOY_GITHUB = $true;
  $env:CI_DEPLOY_FTP = $true;
	$env:CI_DEPLOY_WebHook = $true;
	$env:CI_DEPLOY_WebDeploy = $true;

	$env:CI_DEPLOY_GITHUB_ISDRAFT = "false";
	$env:CI_DEPLOY_GITHUB_ISPRERELEASE = "false";

	if($matches[1] -ieq "draft") {
		$env:CI_DEPLOY_GITHUB_ISDRAFT = "true";
	}
	if($matches[1] -ieq "pre-release") {
		$env:CI_DEPLOY_GITHUB_ISDRAFT = "false";
	}

} else {
	# Do not assign a release number or deploy
  $env:CI_DEPLOY_NUGET = $false;
  $env:CI_DEPLOY_GITHUB = $false;
  $env:CI_DEPLOY_FTP = $false;
	$env:CI_DEPLOY_WebHook = $false;
	$env:CI_DEPLOY_WebDeploy = $false;
}