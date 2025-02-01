# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["webservice.csproj", "."]
RUN dotnet restore "./webservice.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./webservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./webservice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Switch to root to install PostgreSQL client utilities (pg_isready)
USER root

# Install curl in the final image
RUN apt-get update && apt-get install -y curl

# Install PostgreSQL client utilities (pg_isready)
RUN apt-get update && apt-get install -y postgresql-client

COPY --from=publish /app/publish .

# Copy the wait-for-postgres.sh script into the container
COPY ./wait-for-postgres.sh /webService/wait-for-postgres.sh

# Make the wait-for-postgres.sh script executable
RUN chmod +x /webService/wait-for-postgres.sh

# Switch back to the app user for the remaining work
USER app

# Set the entrypoint to run the wait-for-postgres.sh script and then start the web service
ENTRYPOINT ["/bin/bash", "/webService/wait-for-postgres.sh", "dotnet", "webservice.dll"]