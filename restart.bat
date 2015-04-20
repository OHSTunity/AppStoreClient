@echo off
staradmin kill all

REM Start Launcher
call "%~dp0..\AppStoreClient\run.bat"

call "%~dp0run.bat"