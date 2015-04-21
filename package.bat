@echo off
SETLOCAL
SET output=artifacts

REM Create empty folder where will will create the file structure to be packed (zipped)
IF EXIST "%~dp0temp\" (
	rd "%~dp0temp" /S /Q || (GOTO ERROR)
) else (
 md "%~dp0temp" || (GOTO ERROR)
)

IF EXIST "%~dp0%output%\" (
	rd "%~dp0%output%" /S /Q || (GOTO ERROR)
)


REM Get projectname from current folder name
for %%a in (.) do set projectname=%%~na

REM Prepare Executables
IF EXIST "%~dp0bin\Debug\*.exe" (
	md "%~dp0temp\app" || (GOTO ERROR)
	xcopy "%~dp0bin\Debug\*.*" "%~dp0temp\app" || (GOTO ERROR)
) else (
	echo No Executable found
	GOTO ERROR
)


REM Prepare wwwroot
md "%~dp0temp\wwwroot" || (GOTO ERROR)
xcopy "%~dp0src\%projectname%\wwwroot" "%~dp0temp\wwwroot" /s /e || (GOTO ERROR)

REM Copy icon and config
xcopy "%~dp0src\%projectname%\package\*.png" "%~dp0temp" || (GOTO ERROR)
xcopy "%~dp0src\%projectname%\package\*.config" "%~dp0temp" || (GOTO ERROR)

REM Get folder name for the zip name
REM for %%a in ("%~dp0.") do set currentfolder=%%~na

IF NOT EXIST "%~dp0%output%" (
 md "%~dp0%output%" || (GOTO ERROR)
)

REM Zipp-it
CD "%~dp0temp" || (GOTO ERROR)

zip -r "..\%output%\%projectname%" *.*  || (GOTO ERROR)

Echo .
Echo Package Successfully created (%output%\%projectname%.zip)

REM TODO Jump back to previous dir
GOTO :CLEANUP

:ERROR

:CLEANUP

CD "%~dp0"

IF EXIST "%~dp0temp\" (
	rd "%~dp0temp" /S /Q 
) 


:END

ENDLOCAL
