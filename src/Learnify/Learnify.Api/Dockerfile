FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Learnify/Learnify.Api/Learnify.Api.csproj", "src/Learnify/Learnify.Api/"]
COPY ["src/Learnify/Learnify.Core/Learnify.Core.csproj", "src/Learnify/Learnify.Core/"]
COPY ["src/Learnify/Learnify.Infrastructure/Learnify.Infrastructure.csproj", "src/Learnify/Learnify.Infrastructure/"]
RUN dotnet restore "src/Learnify/Learnify.Api/Learnify.Api.csproj"
COPY . .
WORKDIR "/src/src/Learnify/Learnify.Api"
RUN dotnet build "Learnify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Learnify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Learnify.Api.dll"]
