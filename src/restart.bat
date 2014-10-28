@echo off
staradmin kill all

REM Start Launcher
call "%~dp0..\..\Launcher\run.bat"

REM Start AppStore Server
REM call "%~dp0..\..\AppStoreServer\src\run.bat"


call "%~dp0run.bat"