#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Basket/Basket.Host/Basket.Host.csproj", "Basket/Basket.Host/"]
RUN dotnet restore "Basket/Basket.Host/Basket.Host.csproj"
COPY . .
WORKDIR "/src/Basket/Basket.Host"
RUN dotnet build "Basket.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Host.dll"]