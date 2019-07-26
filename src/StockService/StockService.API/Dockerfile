FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/StockService/StockService.API/StockService.API.csproj", "src/StockService/StockService.API/"]
COPY ["lib/LibWebAPI/LibWebAPI.csproj", "lib/LibWebAPI/"]
COPY ["lib/LibOpenTracing/LibOpenTracing.csproj", "lib/LibOpenTracing/"]
RUN dotnet restore "src/StockService/StockService.API/StockService.API.csproj"
COPY . .
WORKDIR "/src/src/StockService/StockService.API"
RUN dotnet build "StockService.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StockService.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StockService.API.dll"]
