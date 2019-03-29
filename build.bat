::@echo off
@setlocal enableextensions
@cd /d "%~dp0"
echo %cd%
MSBuild SmtpComponent.sln /p:Configuration=Release
move SmtpSSL\bin\Release\SmtpSSL.dll "%programfiles(x86)%\OpenSpan\OpenSpan Plug-in for Microsoft Visual Studio 2015"
pause