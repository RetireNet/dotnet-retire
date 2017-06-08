IF EXIST "./deploy" RMDIR "./deploy" /s /q
dotnet clean dotnet-retire
dotnet restore dotnet-retire
dotnet build dotnet-retire -c Release
dotnet pack dotnet-retire -o ..\deploy -c Release
