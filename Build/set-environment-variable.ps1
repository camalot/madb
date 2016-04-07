Param (
   [string] $Name,
   [string] $Value
);

(Set-Item -Path Env:\${Name} -Value $Value);