
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["backend/MediaVerse/MediaVerse.Client.Api/MediaVerse.Client.Api.csproj", "MediaVerse.Client.Api/"]
COPY ["backend/MediaVerse/MediaVerse.Client.Application/MediaVerse.Client.Application.csproj", "MediaVerse.Client.Application/"]
COPY ["backend/MediaVerse/MediaVerse.Infrastructure/MediaVerse.Infrastructure.csproj", "MediaVerse.Infrastructure/"]
COPY ["backend/MediaVerse/MediaVerse.Domain/MediaVerse.Domain.csproj", "MediaVerse.Domain/"]
RUN dotnet restore "MediaVerse.Client.Api/MediaVerse.Client.Api.csproj"
COPY backend/MediaVerse/. .
WORKDIR "/src/MediaVerse.Client.Api"
RUN dotnet build "MediaVerse.Client.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediaVerse.Client.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM node:lts-alpine AS build-client

WORKDIR /usr/src/app

COPY frontend/. .


ENV VITE_API_URL=

RUN npm install && npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build-client /usr/src/app/dist wwwroot/

ENTRYPOINT ["dotnet", "MediaVerse.Client.Api.dll"]
