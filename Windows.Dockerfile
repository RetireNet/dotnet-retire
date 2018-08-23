FROM microsoft/dotnet:2.1-sdk

# Pre-pre dotnet
RUN dotnet --info
RUN dotnet tool install -g Cake.Tool --version 0.30.0
RUN dotnet tool list -g
WORKDIR \build
COPY .\ .
RUN dotnet cake
