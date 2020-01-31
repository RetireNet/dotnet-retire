
FROM ubuntu:16.04

# Enable SSL & Install .NET Core
RUN apt-get update \
    && apt-get install -y apt-transport-https curl tzdata git \
    && apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF \
    && echo "deb http://download.mono-project.com/repo/ubuntu stable-xenial main" | tee /etc/apt/sources.list.d/mono-official-stable.list \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main" > /etc/apt/sources.list.d/dotnetdev.list' \
    && apt-get update \
    && apt-get install -y --no-install-recommends dotnet-sdk-3.1 unzip mono-devel \
	&& rm -rf /var/lib/apt/lists/* \
    && apt-get clean \
    && mkdir -p /opt/nuget \
    && curl -Lsfo /opt/nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE 1

WORKDIR /build
COPY ./ .
RUN dotnet tool install -g Cake.Tool --version 0.30.0
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN dotnet tool list -g
ARG cakeargs=""
RUN dotnet cake
