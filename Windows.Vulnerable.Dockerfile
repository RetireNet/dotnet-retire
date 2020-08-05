FROM mcr.microsoft.com/dotnet/core/sdk:3.1


USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Users\ContainerUser\.dotnet\tools"
USER ContainerUser

WORKDIR /dotnet-retire
COPY ./assert-cmd.bat ./

COPY ./src/RetireNet.Packages.Tool ./
RUN dotnet build
RUN dotnet pack -o ../deploy /p:Version=999.0.0
RUN dotnet tool install -g dotnet-retire --add-source ../deploy --version=999.0.0
RUN dotnet tool list -g

WORKDIR /VulnerableApp
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--loglevel=debug"
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire"
RUN /dotnet-retire/assert-cmd.bat 0 "dotnet-retire" "--ignore-failures"

WORKDIR /VulnerableRunTimeWebApp
COPY SampleProjects/VulnerableRunTimeWebApp/VulnerableRunTimeWebApp.csproj ./
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--loglevel=debug"
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire"
RUN /dotnet-retire/assert-cmd.bat 0 "dotnet-retire" "--ignore-failures"

WORKDIR /VulnerableSolution
COPY SampleProjects/VulnerableSolution.sln ./VulnerableSolution.sln
COPY SampleProjects/VulnerableRunTimeWebApp/VulnerableRunTimeWebApp.csproj ./VulnerableRunTimeWebApp/VulnerableRunTimeWebApp.csproj
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./VulnerableApp/VulnerableApp.csproj
COPY SampleProjects/VulnerableConsoleApp/VulnerableConsoleApp.csproj ./VulnerableConsoleApp/VulnerableConsoleApp.csproj
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--loglevel=debug"
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire"
RUN /dotnet-retire/assert-cmd.bat 0 "dotnet-retire" "--ignore-failures"

WORKDIR /
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution"
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution/VulnerableSolution.sln"
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution/VulnerableApp"
RUN /dotnet-retire/assert-cmd.bat 3 "dotnet-retire" "--path=/VulnerableSolution/VulnerableApp/VulnerableApp.csproj"
RUN /dotnet-retire/assert-cmd.bat 4 "dotnet-retire" "--path=/Not/Existing/Path"
