FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Profile/Profile.Api/Profile.Api.csproj", "src/Profile/Profile.Api/"]
COPY ["src/Notification/Notification.csproj", "src/Notification/"]
COPY ["src/Contracts/Contracts/Contracts.csproj", "src/Contracts/Contracts/"]
COPY ["src/Profile/Profile.Core/Profile.Core.csproj", "src/Profile/Profile.Core/"]
COPY ["src/General/General/General.csproj", "src/General/General/"]
COPY ["src/Profile/Profile.Infrastructure/Profile.Infrastructure.csproj", "src/Profile/Profile.Infrastructure/"]
RUN dotnet restore "src/Profile/Profile.Api/Profile.Api.csproj"
COPY . .
WORKDIR "/src/src/Profile/Profile.Api"
RUN dotnet build "Profile.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Profile.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profile.Api.dll"]
