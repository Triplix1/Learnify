FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Storage/BlobStorage.Api/BlobStorage.Api.csproj", "src/Storage/BlobStorage.Api/"]
COPY ["src/Contracts/Contracts/Contracts.csproj", "src/Contracts/Contracts/"]
COPY ["src/General/General/General.csproj", "src/General/General/"]
COPY ["src/Storage/BlobStorage.Core/BlobStorage.Core.csproj", "src/Storage/BlobStorage.Core/"]
RUN dotnet restore "src/Storage/BlobStorage.Api/BlobStorage.Api.csproj"
COPY . .
WORKDIR "/src/src/Storage/BlobStorage.Api"
RUN dotnet build "BlobStorage.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BlobStorage.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlobStorage.Api.dll"]
