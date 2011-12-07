@echo off
setlocal
pushd "%~dp0"
call build && call :pack
goto :EOF

:pack
setlocal
pushd "%~dp0pkg"
if not exist base\lib\net40 md base\lib\net40
copy ..\COPYING*.txt base > nul ^
 && copy ..\bin\Release base\lib\net40 > nul ^
 && nuget pack Eggado.nuspec -BasePath base
goto :EOF
