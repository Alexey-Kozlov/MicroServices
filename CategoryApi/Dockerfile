FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 5024

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CategoryApi/CategoryAPI.csproj", "CategoryApi/"]
RUN dotnet restore "CategoryApi/CategoryAPI.csproj"
COPY . .
WORKDIR "/src/CategoryApi"
RUN dotnet build "CategoryAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CategoryAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CategoryAPI.dll"]