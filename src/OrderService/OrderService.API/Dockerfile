FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/OrderService/OrderService.API/OrderService.API.csproj", "src/OrderService/OrderService.API/"]
COPY ["src/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "src/OrderService/OrderService.Infrastructure/"]
COPY ["src/OrderService/OrderService.Domain/OrderService.Domain.csproj", "src/OrderService/OrderService.Domain/"]
COPY ["lib/LibMediator/LibMediator.csproj", "lib/LibMediator/"]
RUN dotnet restore "src/OrderService/OrderService.API/OrderService.API.csproj"
COPY . .
WORKDIR "/src/src/OrderService/OrderService.API"
RUN dotnet build "OrderService.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OrderService.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
