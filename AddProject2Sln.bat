@echo off
set sln=%~dp0CFET2APP.sln
set ext=.csproj
cd %1
for /r %%a in (*%ext%) do (
	echo adding: %%adding
	dotnet sln %sln% add %%a
)