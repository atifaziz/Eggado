@echo off
setlocal
pushd "%~dp0"
for %%i in (Debug Release) do "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild" /p:Configuration=%%i src\Eggado.sln
