# Base image for runtime (ASP.NET)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# Expose PostgreSQL port
EXPOSE 5432 

# Build image for compiling the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["dataService.csproj", "."]
RUN dotnet restore "./dataService.csproj"

# Copy the rest of the application and build it
COPY . .
WORKDIR "/src"
RUN dotnet build "./dataService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./dataService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Install PostgreSQL
FROM base AS final
USER root
RUN apt-get update \
    && apt-get install -y postgresql postgresql-contrib \
    && apt-get clean

# Set PostgreSQL environment variables
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=postgres
ENV POSTGRES_DB=DataServiceDb

# Create PostgreSQL data directory
RUN mkdir -p /var/lib/postgresql/data

# Expose application port and database port
EXPOSE 80

# Copy the published .NET application to the container
WORKDIR /app
COPY --from=publish /app/publish .

# Start PostgreSQL and the application
CMD ["sh", "-c", "service postgresql start && dotnet dataService.dll"]
