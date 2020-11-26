FROM mcr.microsoft.com/dotnet/sdk:5.0

ENV PATH="/root/.dotnet/tools:${PATH}"



# Pre-pre dotnet
RUN dotnet --info
RUN dotnet tool install -g Cake.Tool --version 0.36.0
RUN dotnet tool list -g
WORKDIR /build
COPY ./ .
RUN dotnet cake
