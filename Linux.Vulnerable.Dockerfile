FROM microsoft/dotnet:2.1-sdk

# copy project
WORKDIR /VulnerableApp
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN dotnet restore

WORKDIR /dotnet-retire
COPY ./src/RetireNet.Packages.Tool ./

RUN dotnet build
RUN dotnet pack -o ../deploy /p:Version=999.0.0

# install dotnet-retire from local feed
WORKDIR /VulnerableApp
RUN dotnet tool install -g dotnet-retire --add-source ../deploy --version=999.0.0
ENV PATH="/root/.dotnet/tools:${PATH}"

RUN dotnet tool list -g

RUN dotnet retire --ignore-failures true --loglevel debug
RUN dotnet retire --ignore-failures true

WORKDIR /VulnerableSolution
COPY SampleProjects/VulnerableSolution.sln ./VulnerableSolution.sln
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./VulnerableApp/VulnerableApp.csproj
COPY SampleProjects/VulnerableConsoleApp/VulnerableConsoleApp.csproj ./VulnerableConsoleApp/VulnerableConsoleApp.csproj

RUN dotnet retire --ignore-failures true
