# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY GradLink.sln ./
COPY GradLink.Domain/GradLink.Domain.csproj ./GradLink.Domain/
COPY GradLink.Application/GradLink.Application.csproj ./GradLink.Application/
COPY GradLink.Infrastructure/GradLink.Infrastructure.csproj ./GradLink.Infrastructure/
COPY GradLink.Api/GradLink.Api.csproj ./GradLink.Api/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build
WORKDIR /src/GradLink.Api
RUN dotnet build -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create directories
RUN mkdir -p /app/data /app/uploads /app/logs

COPY --from=publish /app/publish .

# Railway uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "GradLink.Api.dll"]

