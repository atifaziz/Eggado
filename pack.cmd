@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
call :mkdist && call build && nuget pack -Symbols -OutputDirectory dist
goto :EOF

:mkdist
if exist dist goto :EOF
md dist
if exist dist goto :EOF
echo Error creating package distribution directory
exit /b 1
