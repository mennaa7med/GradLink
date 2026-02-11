# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy backend folder contents
COPY backend/. .

# Restore dependencies using the API project
RUN dotnet restore GradLink.Api/GradLink.Api.csproj

# Publish the API project
RUN dotnet publish GradLink.Api/GradLink.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create required directories
RUN mkdir -p /app/data /app/uploads /app/logs

# Copy published files
COPY --from=build /app/publish .

# Railway uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "GradLink.Api.dll"]
