# escape=`
FROM microsoft/windowsservercore:latest

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue'; "]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.400
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-win-x64.zip
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE 1
RUN Invoke-WebRequest $Env:DOTNET_SDK_DOWNLOAD_URL -OutFile dotnet.zip -TimeoutSec 10;
RUN Expand-Archive dotnet.zip -DestinationPath C:\dotnet
RUN Remove-Item -Force dotnet.zip
ENV PATH="${PATH};C:\\dotnet\\;"
# Pre-pre dotnet
RUN dotnet --info

WORKDIR \build
COPY .\ .
RUN .\build.ps1
