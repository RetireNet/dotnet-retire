FROM microsoft/dotnet:2.1-sdk

ENV PATH="/root/.dotnet/tools:${PATH}"



WORKDIR /dotnet-retire
COPY ./src/RetireNet.Packages.Tool ./
RUN dotnet build
RUN dotnet pack -o ../deploy /p:Version=999.0.0
RUN dotnet tool install -g dotnet-retire --add-source ../deploy --version=999.0.0
RUN dotnet tool list -g

WORKDIR /VulnerableApp
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN dotnet retire loglevel=debug
RUN dotnet retire

WORKDIR /VulnerableRunTimeWebApp
COPY SampleProjects/VulnerableRunTimeWebApp/VulnerableRunTimeWebApp.csproj ./
RUN dotnet retire loglevel=debug
RUN dotnet retire

WORKDIR /VulnerableSolution
COPY SampleProjects/VulnerableSolution.sln ./VulnerableSolution.sln
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./VulnerableApp/VulnerableApp.csproj
COPY SampleProjects/VulnerableConsoleApp/VulnerableConsoleApp.csproj ./VulnerableConsoleApp/VulnerableConsoleApp.csproj
RUN dotnet retire loglevel=debug
RUN dotnet retire

