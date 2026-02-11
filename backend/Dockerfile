# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY backend/. .

RUN dotnet restore GradLink.Api/GradLink.Api.csproj
RUN dotnet publish GradLink.Api/GradLink.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN mkdir -p /app/data /app/uploads /app/logs

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "GradLink.Api.dll"]
