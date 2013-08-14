
@echo sign with certificate
"c:\Program Files\Microsoft SDKs\Windows\v7.1A\Bin\signtool" sign /f "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\keys\CGIRuler2.pfx" /p niltap1 /v :\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\F5\bin\Debug\F5.exe" 

"c:\Program Files\Microsoft SDKs\Windows\v7.1A\Bin\signtool" sign /f "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\keys\CGIRuler2.pfx" /p niltap1 /v "C:\Users\xxlindtp\Documents\GitHub\CgiExperimentF5\F5\bin\Debug\Essential.Diagnostics.dll"

pause