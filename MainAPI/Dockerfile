FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

ENV RABBIT_PRODUCER=/ms/api/ms-rabbit-producer
ENV FRONT_URL=/ms
ENV PRODUCT_API=/ms/api/product
ENV CATEGORY_API=/ms/api/category
ENV ORDER_API=/ms/api/order
ENV IDENTITY_LOGIN=/ms/identity
ENV IDENTITY_TOKEN=/ms/identity/api

WORKDIR /app
EXPOSE 5023

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