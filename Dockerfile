FROM microsoft/dotnet:2.1-sdk as builder

COPY src/Cli/Cli.csproj src/Cli/
COPY src/Core/Core.csproj src/Core/
COPY src/React/React.csproj src/React/
COPY src/Web/Web.csproj src/Web/
COPY tests/E2ETests/E2ETests.csproj tests/E2ETests/
COPY tests/PerformanceTests/PerformanceTests.csproj tests/PerformanceTests/
COPY tests/UnitTests/UnitTests.csproj tests/UnitTests/
COPY street-runner.sln .

RUN dotnet restore

ADD src/ src/
ADD tests/ tests/

RUN dotnet build
RUN dotnet test tests/UnitTests
RUN dotnet publish src/Web/ --output ./out

FROM microsoft/dotnet:2.1-runtime
COPY --from=builder /src/Web/out /app
RUN mkdir /wwwroot -p

ENV ASPNETCORE_URLS=http://+:5000
CMD dotnet app/StreetRunner.Web.dll
