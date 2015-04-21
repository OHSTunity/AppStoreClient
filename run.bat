@echo off

SETLOCAL

REM Get projectname from current folder name
for %%a in (.) do set projectname=%%~na

REM Get Executable
FOR %%i IN ("%~dp0bin\debug\*.exe") DO ( 
SET executable=%%i
)

IF DEFINED executable (
 star -d=default --resourcedir="%~dp0src\%projectname%\wwwroot" "%executable%"
) ELSE (
 ECHO executable was not found.
)


