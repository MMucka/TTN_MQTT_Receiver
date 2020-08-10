# Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

WORKDIR /app
EXPOSE 80

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT dotnet run
