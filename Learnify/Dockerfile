FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Learnify/Learnify.csproj", "Learnify/"]
RUN dotnet restore "Learnify/Learnify.csproj"
COPY . .
WORKDIR "/src/Learnify"
RUN dotnet build "Learnify.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Learnify.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Learnify.dll"]
