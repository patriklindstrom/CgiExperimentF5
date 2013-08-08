Push-Location
Set-Location "D:\Users\Patrik\Documents\GitHub\CgiExperimentF5\F5\bin\Debug"
Copy-Item -Path .\F5.exe -Destination P:\inetpub\wwwroot\cgi
Copy-Item -Path .\F5.exe.config -Destination P:\inetpub\wwwroot\cgi
Pop-Location


