FROM mcr.microsoft.com/dotnet/sdk:5.0

# install dotnet-retire
RUN dotnet tool install -g dotnet-retire

# copy project
COPY VulnerableApp/VulnerableApp.csproj ./VulnerableApp/
WORKDIR ./VulnerableApp
RUN dotnet restore
RUN dotnet retire
