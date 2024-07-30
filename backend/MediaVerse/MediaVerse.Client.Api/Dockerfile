FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MediaVerse.Client.Api/MediaVerse.Client.Api.csproj", "MediaVerse.Client.Api/"]
COPY ["MediaVerse.Client.Application/MediaVerse.Client.Application.csproj", "MediaVerse.Client.Application/"]
COPY ["MediaVerse.Infrastructure/MediaVerse.Infrastructure.csproj", "MediaVerse.Infrastructure/"]
COPY ["MediaVerse.Domain/MediaVerse.Domain.csproj", "MediaVerse.Domain/"]
RUN dotnet restore "MediaVerse.Client.Api/MediaVerse.Client.Api.csproj"
COPY . .
WORKDIR "/src/MediaVerse.Client.Api"
RUN dotnet build "MediaVerse.Client.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediaVerse.Client.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediaVerse.Client.Api.dll"]
