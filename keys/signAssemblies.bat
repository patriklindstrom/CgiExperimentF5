@echo Has to be run as admin
@echo sign with certificate
"c:\Program Files\Microsoft SDKs\Windows\v7.1A\Bin\signtool" sign /f "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\keys\PatrikLindstromAssemblySigningAT_Signature.pfx" /p mypwd /v "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\F5\bin\Debug\F5.exe" 

"c:\Program Files\Microsoft SDKs\Windows\v7.1A\Bin\signtool" sign /f "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\keys\PatrikLindstromAssemblySigningAT_Signature.pfx" /p mypwd /v "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\F5\bin\Debug\Essential.Diagnostics.dll"
@echo copy dll and config from debug
robocopy "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\F5\bin\Debug"  "C:\inetpub\cgi-bin" Essential.Diagnostics.dll F5.exe F5.exe.config

pause