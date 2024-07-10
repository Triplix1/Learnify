FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Identity/AuthIdentity.API/AuthIdentity.API.csproj", "src/Identity/AuthIdentity.API/"]
COPY ["src/General/General/General.csproj", "src/General/General/"]
COPY ["src/Contracts/Contracts/Contracts.csproj", "src/Contracts/Contracts/"]
COPY ["src/Identity/AuthIdentity.Core/AuthIdentity.Core.csproj", "src/Identity/AuthIdentity.Core/"]
RUN dotnet restore "src/Identity/AuthIdentity.API/AuthIdentity.API.csproj"
COPY . .
WORKDIR "/src/src/Identity/AuthIdentity.API"
RUN dotnet build "AuthIdentity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthIdentity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthIdentity.API.dll"]
