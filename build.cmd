dotnet restore dotnet-retire.sln
dotnet build dotnet-retire.sln -c Release
cd Tests
dotnet xunit -c Release
cd ..
