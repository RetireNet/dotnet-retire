FROM mcr.microsoft.com/dotnet/sdk:5.0


USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Users\ContainerUser\.dotnet\tools"
USER ContainerUser

WORKDIR /dotnet-retire
COPY ./src/RetireNet.Packages.Tool ./
RUN dotnet build
RUN dotnet pack -o ../deploy /p:Version=999.0.0
RUN dotnet tool install -g dotnet-retire --add-source ../deploy --version=999.0.0
RUN dotnet tool list -g

WORKDIR /
USER ContainerAdministrator
RUN rmdir /s /q dotnet-retire
USER ContainerUser

WORKDIR /VulnerableApp
COPY /assert-cmd.bat ./assert-cmd.bat
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN assert-cmd.bat 3 "dotnet-retire" "--loglevel=debug"
RUN assert-cmd.bat 3 "dotnet-retire"
RUN assert-cmd.bat 0 "dotnet-retire" "--ignore-failures"

WORKDIR /VulnerableRunTimeWebApp
COPY /assert-cmd.bat ./assert-cmd.bat
COPY SampleProjects/VulnerableRunTimeWebApp/VulnerableRunTimeWebApp.csproj ./
RUN assert-cmd.bat 3 "dotnet-retire" "--loglevel=debug"
RUN assert-cmd.bat 3 "dotnet-retire"
RUN assert-cmd.bat 0 "dotnet-retire" "--ignore-failures"

WORKDIR /VulnerableSolution
COPY /assert-cmd.bat ./assert-cmd.bat
COPY SampleProjects/VulnerableSolution.sln ./VulnerableSolution.sln
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./VulnerableApp/VulnerableApp.csproj
COPY SampleProjects/VulnerableConsoleApp/VulnerableConsoleApp.csproj ./VulnerableConsoleApp/VulnerableConsoleApp.csproj
RUN assert-cmd.bat 3 "dotnet-retire" "--loglevel=debug"
RUN assert-cmd.bat 3 "dotnet-retire"
RUN assert-cmd.bat 0 "dotnet-retire" "--ignore-failures"

WORKDIR /
COPY /assert-cmd.bat ./assert-cmd.bat
RUN assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution"
RUN assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution/VulnerableSolution.sln"
RUN assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution/VulnerableApp"
RUN assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution/VulnerableApp/VulnerableApp.csproj"
RUN assert-cmd.bat 4 "dotnet-retire" "--path=/Not/Existing/Path"
