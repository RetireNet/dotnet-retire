FROM microsoft/dotnet:2.1-sdk

# copy project
WORKDIR VulnerableApp
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN dotnet restore

WORKDIR ../dotnet-retire
COPY ./dotnet-retire ./

RUN dotnet build
RUN dotnet pack -o ../deploy /p:Version=999.0.0

# install dotnet-retire from local feed
WORKDIR ../VulnerableApp
RUN dotnet tool install -g dotnet-retire --add-source ../deploy --version=999.0.0

RUN dotnet tool list -g

RUN dotnet retire
