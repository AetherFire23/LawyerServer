$PSScriptRoot
Set-Location $PSScriptRoot
Remove-Item "Migrations" -Confirm:$false -Recurse -Force
dotnet.exe ef migrations add initial

