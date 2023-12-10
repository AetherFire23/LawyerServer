 $PSScriptRoot

Start-Process -FilePath "$PSScriptRoot\..\ProcedureMakerServer.sln"

Set-Location $PSScriptRoot
Set-Location "..\..\lawyer-procedure"
code .

exit