@echo off
dotnet restore dotnet-retire.sln
IF %errorlevel% neq 0 exit /b %errorlevel%

dotnet build dotnet-retire.sln -c Release
IF %errorlevel% neq 0 exit /b %errorlevel%

cd Tests
dotnet test -c Release
IF %errorlevel% neq 0 exit /b %errorlevel%
cd ..
