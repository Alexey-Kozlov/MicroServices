FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV IDENTITY_TOKEN=http://ms-identity-api-service:5020/identity/api/CheckToken

ENV RABBIT_SERVICE_NAME=rabbitmq-cluster-ip-service
ENV RABBIT_QUEUE_NAME=MServices
ENV RABBIT_SERVICE_PORT=5672

WORKDIR /app
EXPOSE 5021

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RabbitProducer/RabbitProducer.csproj", "RabbitProducer/"]
RUN dotnet restore "RabbitProducer/RabbitProducer.csproj"
COPY . .
WORKDIR "/src/RabbitProducer"
RUN dotnet build "RabbitProducer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitProducer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitProducer.dll"]