FROM mcr.microsoft.com/dotnet/sdk:5.0

ENV PATH="/root/.dotnet/tools:${PATH}"

WORKDIR /dotnet-retire
COPY ./src/RetireNet.Packages.Tool ./
RUN dotnet build
RUN dotnet pack -o ../deploy /p:Version=999.0.0
RUN dotnet tool install -g dotnet-retire --add-source ../deploy --version=999.0.0
RUN dotnet tool list -g
RUN rm -rf /dotnet-retire

WORKDIR /VulnerableApp
COPY /assert-cmd.sh ./assert-cmd.sh
RUN chmod +x assert-cmd.sh
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./
RUN ./assert-cmd.sh 3 "dotnet retire --loglevel=debug"
RUN ./assert-cmd.sh 3 "dotnet retire"
RUN ./assert-cmd.sh 0 "dotnet retire --ignore-failures"

WORKDIR /VulnerableRunTimeWebApp
COPY /assert-cmd.sh ./assert-cmd.sh
RUN chmod +x assert-cmd.sh
COPY SampleProjects/VulnerableRunTimeWebApp/VulnerableRunTimeWebApp.csproj ./
RUN ./assert-cmd.sh 3 "dotnet retire --loglevel=debug"
RUN ./assert-cmd.sh 3 "dotnet retire"
RUN ./assert-cmd.sh 0 "dotnet retire --ignore-failures"

WORKDIR /VulnerableSolution
COPY /assert-cmd.sh ./assert-cmd.sh
RUN chmod +x assert-cmd.sh
COPY SampleProjects/VulnerableSolution.sln ./VulnerableSolution.sln
COPY SampleProjects/VulnerableApp/VulnerableApp.csproj ./VulnerableApp/VulnerableApp.csproj
COPY SampleProjects/VulnerableConsoleApp/VulnerableConsoleApp.csproj ./VulnerableConsoleApp/VulnerableConsoleApp.csproj
RUN ./assert-cmd.sh 3 "dotnet retire --loglevel=debug"
RUN ./assert-cmd.sh 3 "dotnet retire"
RUN ./assert-cmd.sh 0 "dotnet retire --ignore-failures"

WORKDIR /
COPY /assert-cmd.sh ./assert-cmd.sh
RUN chmod +x assert-cmd.sh
RUN ./assert-cmd.sh 3 "dotnet retire --path /VulnerableSolution"
RUN ./assert-cmd.sh 3 "dotnet retire --path /VulnerableSolution/VulnerableSolution.sln"
RUN ./assert-cmd.sh 3 "dotnet retire --path /VulnerableSolution/VulnerableApp"
RUN ./assert-cmd.sh 3 "dotnet retire --path /VulnerableSolution/VulnerableApp/VulnerableApp.csproj"
RUN ./assert-cmd.sh 4 "dotnet retire --path /Not/Existing/Path"
