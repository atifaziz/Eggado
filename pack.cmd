@echo off
setlocal
pushd "%~dp0"
call build && call :pack
goto :EOF

:pack
setlocal
pushd "%~dp0pkg"
if exist base rd base /s /q
if not errorlevel 0 exit /b %errorlevel%
set net40=base\lib\net40
if not exist "%net40%" md "%net40%"
if not errorlevel 0 exit /b %errorlevel%
copy ..\COPYING*.txt base > nul ^
 && copy ..\bin\Release "%net40%" > nul ^
 && nuget pack Eggado.nuspec -BasePath base
goto :EOF
