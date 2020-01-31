FROM mcr.microsoft.com/dotnet/core/sdk:3.1

ENV PATH="/root/.dotnet/tools:${PATH}"



# Pre-pre dotnet
RUN dotnet --info
RUN dotnet tool install -g Cake.Tool --version 0.36.0
RUN dotnet tool list -g
WORKDIR /build
COPY ./ .
RUN dotnet cake
