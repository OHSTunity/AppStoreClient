@echo off

SETLOCAL

REM Get Executable
FOR %%i IN ("%~dp0bin\debug\*.exe") DO ( 
SET executable=%%i
)

IF DEFINED executable (
 star -d=default --resourcedir="%~dp0src\AppStoreClient\wwwroot" "%executable%"
) ELSE (
 ECHO executable was not found.
)

