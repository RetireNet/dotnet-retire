FROM microsoft/dotnet:2.1-sdk

# copy project
WORKDIR VulnerableApp
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN dotnet restore

WORKDIR ../dotnet-retire
COPY ./dotnet-retire ./
RUN dotnet restore
RUN dotnet pack -o ../Deploy /p:Version=2.3-PR

# install dotnet-retire from local feed
WORKDIR ../VulnerableApp
RUN dotnet tool install -g dotnet-retire --add-source ../Deploy --version=2.3-PR

RUN dotnet retire
