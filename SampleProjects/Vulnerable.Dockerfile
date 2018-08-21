FROM microsoft/dotnet:2.1-sdk

# install dotnet-retire
RUN dotnet tool install -g dotnet-retire

# copy project
COPY VulnerableApp/VulnerableApp.csproj ./VulnerableApp/
WORKDIR ./VulnerableApp
RUN dotnet restore
RUN dotnet retire
