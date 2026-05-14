# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /src
ENV DOTNET_NOLOGO=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=true \
    NUGET_XMLDOC_MODE=skip

COPY *.slnx ./
COPY RagWebApi/*.csproj ./RagWebApi/
COPY RagWebApi.UnitTests/*.csproj ./RagWebApi.UnitTests/

RUN dotnet restore RagWebApi.slnx

COPY . .

RUN dotnet build "RagWebApi/RagWebApi.csproj" -c Release

RUN dotnet publish "RagWebApi/RagWebApi.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_URLS="http://+:6100" \
    ASPNETCORE_ENVIRONMENT="Development" \
    DOTNET_NOLOGO=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=true \
    ASPNETCORE_HTTPS_PORT=""

RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*

COPY --from=builder /app/publish .
EXPOSE 4100

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:6100 || exit 1

ENTRYPOINT ["dotnet", "RagWebApi.dll"]
