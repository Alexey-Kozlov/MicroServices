FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

ENV RABBIT_SERVICE_NAME=rabbitmq-cluster-ip-service
ENV RABBIT_QUEUE_NAME=MServices
ENV RABBIT_SERVICE_PORT=5672

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RabbitConsumer/RabbitConsumer.csproj", "RabbitConsumer/"]
RUN dotnet restore "RabbitConsumer/RabbitConsumer.csproj"
COPY . .
WORKDIR "/src/RabbitConsumer"
RUN dotnet build "RabbitConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitConsumer.dll"]