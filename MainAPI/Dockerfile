FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

ENV RABBIT_PRODUCER=http://ms-rabbit-producer-service:5021/rabbitsend
ENV PRODUCT_API=http://ms-product-api-service:5023
ENV CATEGORY_API=http://ms-category-api-service:5024
ENV ORDER_API=http://ms-order-api-service:5025
ENV IDENTITY_TOKEN=http://ms-identity-api-service:5020/identity/api/CheckToken

WORKDIR /app
EXPOSE 5022

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MainAPI/MainAPI.csproj", "MainAPI/"]
RUN dotnet restore "MainAPI/MainAPI.csproj"
COPY . .
WORKDIR "/src/MainAPI"
RUN dotnet build "MainAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainAPI.dll"]