echo off
dotnet restore dotnet-retire.sln
dotnet build dotnet-retire.sln
cd Tests
dotnet xunit
cd ..
echo on