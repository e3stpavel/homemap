FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

WORKDIR /app

COPY . .

# Install dependencies
RUN dotnet restore

# Install ef core tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Run migrations
#  why I use mkdir: https://stackoverflow.com/a/50786651
RUN mkdir -v data
RUN dotnet ef database update --project src/Infrastructure/Infrastructure.Data/Infrastructure.Data.csproj --startup-project src/WebAPI/WebAPI.csproj

FROM base AS dev

CMD dotnet watch run --project src/WebAPI/WebAPI.csproj --urls https://*:7185;http://*:8080
EXPOSE 7185 8080

FROM base AS build

# Build project
RUN dotnet publish src/WebAPI/WebAPI.csproj -c Release -o .output

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS prod

WORKDIR /app

COPY --from=build /app/.output .
COPY --from=build /app/src/WebAPI/data ./data

# make data.db writable
RUN chmod 777 ./data
RUN chmod 777 ./data/data.db

# who knows what is this
RUN addgroup -S runner && adduser -S runner -G runner
USER runner

CMD dotnet Homemap.WebAPI.dll
EXPOSE 8080
