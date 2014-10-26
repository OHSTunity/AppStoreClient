@echo off
staradmin kill all

REM Start Launcher
call "%~dp0..\..\Launcher\run.bat"

REM Start AppStore Server
call "%~dp0..\..\AppStoreServerApp\src\run.bat"


call "%~dp0run.bat"